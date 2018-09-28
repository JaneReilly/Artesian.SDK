using Artesian.SDK.Dto;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    public enum MarketDataType
    {
        ActualTimeSerie = 1,
        MarketAssessment = 2,
        VersionedTimeSerie = 3
    }

    public enum AddOperationResult
    {
        AssessmentAdded = 0
      , ProductAlreadyPresent = 1
      , IllegalReferenceDate = 2
      , ValueAdded = 3
      , TimeAlreadyPresent = 4
    }

    public interface IMarketData
    {
        //Metadata members
        int MarketDataId { get; }
        MarketDataIdentifier Id { get; }
        string DataTimezone { get; }
        MarketDataType Type { get; }
        Granularity Granularity { get; }
        string Timezone { get; }
        Dictionary<string, List<string>> Tags { get; }


        //Methods
        Task Create(MarketDataIdentifier id);
        AddOperationResult AddData<T>(LocalDate localDate, T value) where T : IDictionary, new();
        //Aggiungere altri tipi



        ////Versioned
        //public LocalDateTime? SelectedVersion { get; protected set; }
        //private Dictionary<LocalDateTime, double?> _values = new Dictionary<LocalDateTime, double?>();
        //public IReadOnlyDictionary<LocalDateTime, double?> Values { get; private set; }

        ////Actual
        //private Dictionary<LocalDateTime, double?> _values = new Dictionary<LocalDateTime, double?>();
        //public IReadOnlyDictionary<LocalDateTime, double?> Values { get; private set; }

        ////Assessment
        //public List<AssessmentElement> Assessments { get; protected set; }
    }

}
