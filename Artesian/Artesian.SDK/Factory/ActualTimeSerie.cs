using Ark.Tools.Nodatime.Intervals;
using Artesian.SDK.Dto;
using Artesian.SDK.Service;
using EnsureThat;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DateInterval = Ark.Tools.Nodatime.Intervals.DateInterval;

namespace Artesian.SDK.Factory
{
    public class ActualTimeSerie : IMarketData
    {
        //private static Logger _logger = LogManager.GetCurrentClassLogger();

        //Class members
        private readonly IMetadataService _metadataService;
        private MarketDataEntity.Output _entity;
        private MarketDataEntity.Input _metadata;

        private Dictionary<LocalDateTime, double?> _values = new Dictionary<LocalDateTime, double?>();
        public IReadOnlyDictionary<LocalDateTime, double?> Values { get; private set; }

        //Metadata members
        public int MarketDataId { get { return _entity == null ? 0 : _entity.MarketDataId; } }

        public MarketDataIdentifier Identifier { get; protected set; }

        public string DataTimezone
        {
            get
            {
                if (_entity?.OriginalGranularity.IsTimeGranularity() == true)
                    return "UTC";
                else
                    return _entity?.OriginalTimezone;
            }
        }

        public MarketDataType Type => throw new NotImplementedException();

        public Granularity Granularity { get { return _entity == null ? default : _entity.OriginalGranularity; } }

        public string Timezone => _entity?.OriginalTimezone;

        public Dictionary<string, List<string>> Tags { get { return _entity?.Tags; } }

        public ActualTimeSerie(IMetadataService metadataService)
        {
            EnsureArg.IsNotNull(metadataService, nameof(metadataService));

            _metadataService = metadataService;
            Values = new ReadOnlyDictionary<LocalDateTime, double?>(_values);
        }

        public Task Create(string provider, string name, CancellationToken ctk = default)
        {
            var id = new MarketDataIdentifier(provider, name);
            return Create(id, ctk);
        }

        public async Task Create(MarketDataIdentifier id, CancellationToken ctk = default)
        {
            Identifier = id;
            _entity = await _metadataService.ReadMarketDataRegistryAsync(id, ctk);
        }

        #region Interface Methods
        public void ClearData()
        {
            _values = new Dictionary<LocalDateTime, double?>();
            Values = new ReadOnlyDictionary<LocalDateTime, double?>(_values);
        }

        public async Task Register(MarketDataEntity.Input metadata)
        {
            EnsureArg.IsNotNull(metadata, nameof(metadata));
            EnsureArg.IsTrue(metadata.ProviderName == null || metadata.ProviderName == this.Identifier.Provider);
            EnsureArg.IsTrue(metadata.ProviderName == null || metadata.ProviderName == this.Identifier.Provider);
            EnsureArg.IsTrue(metadata.MarketDataName == null || metadata.MarketDataName == this.Identifier.Name);
            EnsureArg.IsNotNullOrWhiteSpace(metadata.OriginalTimezone);

            metadata.ProviderName = this.Identifier.Provider;
            metadata.MarketDataName = this.Identifier.Name;

            if (_entity != null)
                throw new ActualTimeSerieException("Actual Time Serie is already registered with ID {0}", _entity.MarketDataId);
            _entity = await _metadataService.RegisterMarketDataAsync(metadata);
        }

        public async Task<(IMarketData, bool)> IsRegistered()
        {
           if (_entity == null)
               _entity = await _metadataService.ReadMarketDataRegistryAsync(this.Identifier);

            if (_entity != null)
                return (_entity as IMarketData, true);

            return (null, false);
        }

        public async Task<MarketDataEntity.Input> LoadMetadata()
        {
            _metadata = _entity;
            return _metadata;
        }

        public async Task Update()
        {
            if (_entity == null)
                throw new ActualTimeSerieException("Actual Time Serie is not yet registered");

            _entity = await _metadataService.UpdateMarketDataAsync(_metadata);
        }
        #endregion

        #region Write
        public AddTimeSerieOperationResult AddData(LocalDate localDate, double? value)
        {
            Ensure.Any.IsNotNull(_entity);

            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new ActualTimeSerieException("This MarketData has Time granularity. Use AddData(Instant time, double? value)");

            var localTime = localDate.AtMidnight();

            return _add(localTime, value);
        }

        public AddTimeSerieOperationResult AddData(Instant time, double? value)
        {
            Ensure.Any.IsNotNull(_entity);

            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new ActualTimeSerieException("This MarketData has Date granularity. Use AddData(LocalDate date, double? value)");

            var localTime = time.InUtc().LocalDateTime;

            return _add(localTime, value);
        }

        private AddTimeSerieOperationResult _add(LocalDateTime localTime, double? value)
        {
            if (_values.ContainsKey(localTime))
                return AddTimeSerieOperationResult.TimeAlreadyPresent;

            if (_entity.OriginalGranularity.IsTimeGranularity())
            {
                var period = ArtesianUtils.MapTimePeriod(_entity.OriginalGranularity);
                if (!TimeInterval.IsStartOfInterval(localTime, period))
                    throw new ArtesianSdkClientException("Trying to insert Time {0} with wrong format to serie {1}. Should be of period {2}", localTime, Identifier, period);
            }
            else
            {
                var period = ArtesianUtils.MapDatePeriod(_entity.OriginalGranularity);
                if (!DateInterval.IsStartOfInterval(localTime, period))
                    throw new ArtesianSdkClientException("Trying to insert Time {0} with wrong format to serie {1}. Should be of period {2}", localTime, Identifier, period);
            }

            _values.Add(localTime, value);
            return AddTimeSerieOperationResult.ValueAdded;
        }

        public ActualTimeSerie EditActual()
        {
            Ensure.Any.IsNotNull(_metadata);

            return this;
        }

        public async Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true)
        {
            Ensure.Any.IsNotNull(_entity);

            if (_values.Any())
            {
                var data = new UpsertCurveData(this.Identifier)
                {
                    Timezone = _entity.OriginalGranularity.IsTimeGranularity() ? "UTC" : _entity.OriginalTimezone,
                    DownloadedAt = downloadedAt,
                    Rows = _values,
                    DeferCommandExecution = deferCommandExecution,
                    DeferDataGeneration = deferDataGeneration
                };

                await _metadataService.UpsertCurveDataAsync(data);
            }
            //else
            //    _logger.Warn("No Data to be saved.");
        }
        #endregion
    }
}
