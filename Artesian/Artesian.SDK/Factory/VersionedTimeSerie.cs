using Artesian.SDK.Common;
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

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// VersionedTimeSerie entity
    /// </summary>
    public class VersionedTimeSerie : MarketData, ITimeserieWritable
    {
        private Dictionary<LocalDateTime, double?> _values = new Dictionary<LocalDateTime, double?>();

        /// <summary>
        /// MarketData Version
        /// </summary>
        public LocalDateTime? SelectedVersion { get; protected set; }

        /// <summary>
        /// MarketData Curve Values
        /// </summary>
        public IReadOnlyDictionary<LocalDateTime, double?> Values { get; private set; }

        /// <summary>
        /// VersionedTimeSerie Constructor
        /// </summary>
        public VersionedTimeSerie(IMetadataService metadataService, MarketDataEntity.Output entity) : base(metadataService, entity)
        {
            Values = new ReadOnlyDictionary<LocalDateTime, double?>(_values);
        }

        /// <summary>
        /// MarketData Set Version
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <param name="version">LocalDateTime</param>
        /// <returns></returns>
        public void SetSelectedVersion(LocalDateTime version)
        {
            if ((SelectedVersion.HasValue) && (Values.Count != 0))
                throw new VersionedTimeSerieException("SelectedVersion can't be changed if curve contains values. Current Version is {0}", SelectedVersion.Value);

            SelectedVersion = version;
        }

        /// <summary>
        /// MarketData ClearData
        /// </summary>
        public void ClearData()
        {
            _values = new Dictionary<LocalDateTime, double?>();
            Values = new ReadOnlyDictionary<LocalDateTime, double?>(_values);
        }

        /// <summary>
        /// VersionedTimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on curve with localDate
        /// </remarks>
        /// <returns>AddTimeSerieOperationResult</returns>
        public AddTimeSerieOperationResult AddData(LocalDate localDate, double? value)
        {
            Ensure.Any.IsNotNull(_entity);

            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new VersionedTimeSerieException("This MarketData has Time granularity. Use AddData(Instant time, double? value)");

            var localTime = localDate.AtMidnight();

            return _add(localTime, value);
        }
        /// <summary>
        /// VersionedTimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on curve with Instant
        /// </remarks>
        /// <returns>AddTimeSerieOperationResult</returns>
        public AddTimeSerieOperationResult AddData(Instant time, double? value)
        {
            Ensure.Any.IsNotNull(_entity);

            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new VersionedTimeSerieException("This MarketData has Date granularity. Use AddData(LocalDate date, double? value)");

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
                if (!localTime.IsStartOfInterval(period))
                    throw new ArtesianSdkClientException("Trying to insert Time {0} with wrong format to serie {1}. Should be of period {2}", localTime, Identifier, period);
            }
            else
            {
                var period = ArtesianUtils.MapDatePeriod(_entity.OriginalGranularity);
                if (!localTime.IsStartOfInterval(period))
                    throw new ArtesianSdkClientException("Trying to insert Time {0} with wrong format to serie {1}. Should be of period {2}", localTime, Identifier, period);
            }

            _values.Add(localTime, value);
            return AddTimeSerieOperationResult.ValueAdded;
        }

        /// <summary>
        /// MarketData Save
        /// </summary>
        /// <remarks>
        /// Save the Data of the current MarketData
        /// </remarks>
        /// <param name="downloadedAt">downloaded at</param>
        /// <param name="deferCommandExecution">deferCommandExecution</param>
        /// <param name="deferDataGeneration">deferDataGeneration</param>
        /// <returns></returns>
        public async Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true)
        {
            //Quale dei due??
            //Ensure.Any.IsNotNull(_entity);
            Ensure.Bool.IsTrue(_entity != null, "Market Data is not registred in Arkive.");

            if (!SelectedVersion.HasValue)
                throw new VersionedTimeSerieException("No Version Has been selected to save Data");

            if (Values.Any())
            {
                var data = new UpsertCurveData(this.Identifier, SelectedVersion.Value)
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
    }
}
