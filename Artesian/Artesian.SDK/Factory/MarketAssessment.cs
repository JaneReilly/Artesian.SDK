using Ark.Tools.Nodatime.Intervals;
using Artesian.SDK.Dto;
using Artesian.SDK.Service;
using EnsureThat;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DateInterval = Ark.Tools.Nodatime.Intervals.DateInterval;

namespace Artesian.SDK.Factory
{
    public class MarketAssessment : IMarketData
    {
        //private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMetadataService _metadataService;
        private MarketDataEntity.Output _entity;

        public List<AssessmentElement> Assessments { get; protected set; }

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

        public MarketAssessment(IMetadataService metadataService)
        {
            EnsureArg.IsNotNull(metadataService, nameof(metadataService));

            _metadataService = metadataService;
            Assessments = new List<AssessmentElement>();
        }

        //Methods

        public Task Create(string provider, string name)
        {
            return Create(new MarketDataIdentifier(provider, name));
        }

        public async Task Create(MarketDataIdentifier id)
        {
            Identifier = id;
            _entity = await _metadataService.ReadMarketDataRegistryAsync(id);
        }

        public void ClearData()
        {
            Assessments.Clear();
        }

        public AddAssessmentOperationResult AddData<T>(LocalDate localDate, T value) where T : IDictionary, new()
        {
            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new MarketAssessmentException("This MarketData has Time granularity. Use AddData(Instant time...)");

            var dictionary = value as Dictionary<string, MarketAssessmentValue>;

            return _addAssessment(localDate.AtMidnight(), dictionary.Keys.First(), dictionary.Values.First());
        }

        public AddAssessmentOperationResult AddData<T>(Instant time, T value) where T : IDictionary, new()
        {
            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new MarketAssessmentException("This MarketData has Date granularity. Use AddData(LocalDate date...)");

            var dictionary = value as Dictionary<string, MarketAssessmentValue>;

            return _addAssessment(time.InUtc().LocalDateTime, dictionary.Keys.First(), dictionary.Values.First());
        }

        private AddAssessmentOperationResult _addAssessment(LocalDateTime reportTime, string product, MarketAssessmentValue value)
        {
            IMarketProduct parsedProduct = default;
            if (!MarketProductBuilder.TryParse(product, out parsedProduct))
                throw new MarketAssessmentException("Given Product <{0}> is invalid and cannot be added", product);

            return _addAssessment(reportTime, parsedProduct, value);
        }

        private AddAssessmentOperationResult _addAssessment(LocalDateTime reportTime, IMarketProduct product, MarketAssessmentValue value)
        {
            switch (product.Type)
            {
                case MarketProductType.Absolute:
                    return _addAssessment(reportTime, (ProductAbsolute)product, value);
                case MarketProductType.Special:
                    return _addAssessment(reportTime, (ProductSpecial)product, value);
                case MarketProductType.Relative:
                    throw new NotSupportedException("Relative Products are not supported");
            }

            throw new NotSupportedException("Invalid Product Type");
        }

        private AddAssessmentOperationResult _addAssessment(LocalDateTime reportTime, ProductAbsolute product, MarketAssessmentValue value)
        {
            if (_entity.OriginalGranularity.IsTimeGranularity())
            {
                var period = ArtesianUtils.MapTimePeriod(_entity.OriginalGranularity);
                if (!TimeInterval.IsStartOfInterval(reportTime, period))
                    throw new MarketAssessmentException("Trying to insert Report Time {0} with wrong format to Assessment {1}. Should be of period {2}", reportTime, Identifier, period);
            }
            else
            {
                var period = ArtesianUtils.MapDatePeriod(_entity.OriginalGranularity);
                if (!DateInterval.IsStartOfInterval(reportTime, period))
                    throw new MarketAssessmentException("Trying to insert Report Time {0} with wrong format to Assessment {1}. Should be of period {2}", reportTime, Identifier, period);
            }


            if (reportTime.Date >= product.ReferenceDate)
                return AddAssessmentOperationResult.IllegalReferenceDate;

            if (Assessments
                    .Any(row => row.ReportTime == reportTime && row.Product.Equals(product)))
                return AddAssessmentOperationResult.ProductAlreadyPresent;

            Assessments.Add(new AssessmentElement(reportTime, product, value));
            return AddAssessmentOperationResult.AssessmentAdded;
        }

        private AddAssessmentOperationResult _addAssessment(LocalDateTime reportTime, ProductSpecial product, MarketAssessmentValue value)
        {
            if (Assessments
                    .Any(row => row.ReportTime == reportTime && row.Product.Equals(product)))
                return AddAssessmentOperationResult.ProductAlreadyPresent;

            Assessments.Add(new AssessmentElement(reportTime, product, value));
            return AddAssessmentOperationResult.AssessmentAdded;
        }

        public async Task Register(MarketDataEntity.Input metadata)
        {
            EnsureArg.IsNotNull(metadata, nameof(metadata));
            EnsureArg.IsTrue(metadata.ProviderName == null || metadata.ProviderName == this.Identifier.Provider);
            EnsureArg.IsTrue(metadata.MarketDataName == null || metadata.MarketDataName == this.Identifier.Name);
            EnsureArg.IsNotNullOrWhiteSpace(metadata.OriginalTimezone);
            EnsureArg.IsTrue(metadata.AggregationRule == AggregationRule.Undefined);

            metadata.ProviderName = this.Identifier.Provider;
            metadata.MarketDataName = this.Identifier.Name;

            if (_entity != null)
                throw new MarketAssessmentException("Market Assessment is already registered with ID {0}", _entity.MarketDataId);
            _entity = await _metadataService.RegisterMarketDataAsync(metadata);
        }

        public async Task<bool> IsRegistered()
        {
            if (_entity == null)
                _entity = await _metadataService.ReadMarketDataRegistryAsync(this.Identifier);

            if (_entity != null)
                return true;

            return false;
        }

        public async Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true)
        {
            Ensure.Any.IsNotNull(_entity);

            if (Assessments.Any())
            {
                var data = new UpsertCurveData(this.Identifier);
                data.Timezone = DataTimezone;
                data.DownloadedAt = downloadedAt;
                data.DeferCommandExecution = deferCommandExecution;
                data.MarketAssessment = new Dictionary<LocalDateTime, IDictionary<string, MarketAssessmentValue>>();

                foreach (var reportTime in Assessments.GroupBy(g => g.ReportTime))
                {
                    var assessments = reportTime.ToDictionary(key => key.Product.ToString(), value => value.Value);
                    data.MarketAssessment.Add(reportTime.Key, assessments);
                }

                await _metadataService.UpsertCurveDataAsync(data);
            }
            //else
            //    _logger.Warn("No Data to be saved.");
        }

        public Task<MarketDataEntity.Input> LoadMetadata()
        {
            throw new NotImplementedException();
        }

        public Task Update()
        {
            throw new NotImplementedException();
        }

        Task<(IMarketData, bool)> IMarketData.IsRegistered()
        {
            throw new NotImplementedException();
        }

        public class AssessmentElement
        {
            public AssessmentElement(LocalDateTime reportTime, IMarketProduct product, MarketAssessmentValue value)
            {
                ReportTime = reportTime;
                Product = product;
                Value = value;
            }

            public LocalDateTime ReportTime { get; set; }
            public IMarketProduct Product { get; set; }
            public MarketAssessmentValue Value { get; set; }
        }


    }
}
