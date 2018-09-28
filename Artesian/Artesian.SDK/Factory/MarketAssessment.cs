using Artesian.SDK.Dto;
using Artesian.SDK.Service;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    public class MarketAssessment : IMarketData
    {
        //private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMetadataService _metadataService;
        private MarketDataEntity.Output _entity;

        public int MarketDataId { get { return _entity == null ? 0 : _entity.MarketDataId; } }

        public MarketDataIdentifier Id { get; protected set; }

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

        public List<AssessmentElement> Assessments { get; protected set; }

        public MarketAssessment(IMetadataService metadataService)
        {
            EnsureArg.IsNotNull(metadataService, nameof(metadataService));

            _metadataService = metadataService;
            Assessments = new List<AssessmentElement>();
        }

        public async Task Create(MarketDataIdentifier id)
        {
            Id = id;
            _entity = await _metadataService.ReadMarketDataRegistryAsync(id);
        }

        //Pacchettizzare poi nella funzione interna... fare prima le time serie!!
        public AddOperationResult AddValue<T>(LocalDate date, T value)
        {
            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new ArkiveSdkClientException("This MarketData has Time granularity. Use AddAssessment(Instant time...)");

            var dict = value as IDictionary<string, MarketAssessmentValue>;

            return _addAssessment(date.AtMidnight(), dict.K, value);
        }



        private AddOperationResult _addAssessment(LocalDateTime localDateTime, double? value)
        {
            if (_values.ContainsKey(localDateTime))
                return AddTimeSerieValueOperationResult.TimeAlreadyPresent;

            if (_entity.OriginalGranularity.IsTimeGranularity())
            {
                var period = ArkiveUtils.MapTimePeriod(_entity.OriginalGranularity);
                if (!TimeInterval.IsStartOfInterval(localDateTime, period))
                    throw new ArkiveSdkClientException("Trying to insert Time {0} with wrong format to serie {1}. Should be of period {2}", localDateTime, ActualTimeSerieID, period);
            }
            else
            {
                var period = ArkiveUtils.MapDatePeriod(_entity.OriginalGranularity);
                if (!DateInterval.IsStartOfInterval(localDateTime, period))
                    throw new ArkiveSdkClientException("Trying to insert Time {0} with wrong format to serie {1}. Should be of period {2}", localDateTime, ActualTimeSerieID, period);
            }

            _values.Add(localDateTime, value);
            return AddTimeSerieValueOperationResult.ValueAdded;
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
