using Artesian.SDK.Dto;
using Artesian.SDK.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artesian.SDK.Factory
{
    public class MarketDataFactory
    {
        private readonly IMetadataService _metadataService;
        
        public MarketDataFactory(IMetadataService metadataService)
        {
            EnsureArg.IsNotNull(metadataService);

            _metadataService = metadataService;

        }

        public IMarketData CreateMarketData(MarketDataType type)
        {
            switch (type)
            {
                case MarketDataType.MarketAssessment:
                    {
                        var actual = new MarketAssessment(_metadataService);
                        await actual.Create(id);
                        return actual;

                        //return new MarketAssessment(_metadataService);
                    }

                case MarketDataType.VersionedTimeSerie:
                    return new VersionedTimeSerie();
                default:
                    throw new ArgumentException("type");
            }
        }
    }
}
