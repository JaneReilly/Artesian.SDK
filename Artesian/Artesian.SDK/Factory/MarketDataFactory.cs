using Artesian.SDK.Dto;
using Artesian.SDK.Service;
using EnsureThat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task CreateMarketData(MarketDataType type, MarketDataIdentifier id, CancellationToken ctk = default)
        {
            //var md = await _metadataService.GetMarketDataReferenceAsync(type, id, ctk);

            //var tupla = await md.IsRegistered();

            //var metadata = await md.LoadMetadata();

            //await md.Register(metadata);

            //await md.Update();

            //var wMd = (md as ActualTimeSerie).EditActual();

            //wMd.AddData()


        }

        public async Task<IMarketData> CreateMarketData(MarketDataType type, string provider, string name, CancellationToken ctk = default)
        {
            switch (type)
            {
                case MarketDataType.ActualTimeSerie:
                    {
                        var actual = new ActualTimeSerie(_metadataService);
                        await actual.Create(provider, name, ctk);
                        return actual;
                    }
                case MarketDataType.MarketAssessment:
                    {
                        var actual = new MarketAssessment(_metadataService);
                        await actual.Create(provider, name);
                        return actual;
                    }
                case MarketDataType.VersionedTimeSerie:
                    {
                        var actual = new VersionedTimeSerie(_metadataService);
                        await actual.Create(provider, name);
                        return actual;
                    }
                default:
                    throw new ArgumentException("type");
            }
        }
    }

    public static class MarketDataServiceExt
    {
        public static async Task<IMarketData> GetMarketDataReferenceAsync(this IMetadataService metadataService, MarketDataType type, MarketDataIdentifier id, CancellationToken ctk = default)
        {
            switch (type)
            {
                case MarketDataType.ActualTimeSerie:
                    {
                        var actual = new ActualTimeSerie(metadataService);
                        await actual.Create(id);
                        return actual;
                    }
                case MarketDataType.MarketAssessment:
                    {
                        var actual = new MarketAssessment(metadataService);
                        await actual.Create(id);
                        return actual;
                    }
                case MarketDataType.VersionedTimeSerie:
                    {
                        var actual = new VersionedTimeSerie(metadataService);
                        await actual.Create(id);
                        return actual;
                    }
                default:
                    throw new ArgumentException("type");
            }
        }

        public static async Task<IMarketData> GetMarketDataReferenceAsync(this IMetadataService metadataService, MarketDataType type, string provider, string name, CancellationToken ctk = default)
        {
            switch (type)
            {
                case MarketDataType.ActualTimeSerie:
                    {
                        var actual = new ActualTimeSerie(metadataService);
                        await actual.Create(provider, name , ctk);
                        return actual;
                    }
                case MarketDataType.MarketAssessment:
                    {
                        var actual = new MarketAssessment(metadataService);
                        await actual.Create(provider, name);
                        return actual;
                    }
                case MarketDataType.VersionedTimeSerie:
                    {
                        var actual = new VersionedTimeSerie(metadataService);
                        await actual.Create(provider, name);
                        return actual;
                    }
                default:
                    throw new ArgumentException("type");
            }
        }
    }
}
