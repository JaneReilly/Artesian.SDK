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

    //TODO DA RIVEDERE
    public enum AddTimeSerieOperationResult
    {
        ValueAdded = 0
      , TimeAlreadyPresent = 1
    }

    public enum AddAssessmentOperationResult
    {
        AssessmentAdded = 0
      , ProductAlreadyPresent = 1
      , IllegalReferenceDate = 2
    }

    public interface IMarketData
    {
        //Metadata members
        int MarketDataId { get; }
        MarketDataIdentifier Identifier { get; }
        string DataTimezone { get; }
        MarketDataType Type { get; }
        Granularity Granularity { get; }
        string Timezone { get; }
        Dictionary<string, List<string>> Tags { get; }


        //Methods
        //Task Create(string provider, string name);
        //Task Create(MarketDataIdentifier id);

        void ClearData();

        Task <MarketDataEntity.Input> LoadMetadata();
        Task Update();
        Task Register(MarketDataEntity.Input metadata);
        Task<(IMarketData, bool)> IsRegistered();

        Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true);
    }

}
