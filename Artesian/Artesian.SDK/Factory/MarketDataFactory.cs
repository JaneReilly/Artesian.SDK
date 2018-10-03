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
    /// <summary>
    /// MetadataService Extension
    /// </summary>
    public static class MetadataServiceExtension
    {
        /// <summary>
        /// Read marketdata entity by id and returns an istance of IMarketData if exists
        /// </summary>
        /// <param name="metadataService">MarketDataIdentifier of markedata to be retrieved</param>
        /// <param name="id">MarketDataIdentifier of markedata to be retrieved</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>IMarketData</returns>
        public static async Task<IMarketData> GetMarketDataReferenceAsync(this IMetadataService metadataService, MarketDataIdentifier id, CancellationToken ctk = default)
        {
            var entity = await metadataService.ReadMarketDataRegistryAsync(id, ctk);

            if (entity == null)
                return new MarketData(metadataService, id);

            switch (entity.Type)
            {
                case MarketDataType.ActualTimeSerie:
                    {
                        var actual = new ActualTimeSerie(metadataService, entity);
                        return actual;
                    }
                case MarketDataType.MarketAssessment:
                    {
                        var marketAssessment = new MarketAssessment(metadataService, entity);
                        return marketAssessment;
                    }
                case MarketDataType.VersionedTimeSerie:
                    {
                        var versioned = new VersionedTimeSerie(metadataService, entity);
                        return versioned;
                    }
                default:
                    throw new NotSupportedException($"The Type '{entity.Type}'is not present");
            }
        }

        /// <summary>
        /// Read marketdata entity by MarketDataIdentifier and returns an istance of IMarketData if exists
        /// </summary>
        /// <param name="metadataService">MarketDataIdentifier of markedata to be retrieved</param>
        /// <param name="provider">MarketDataIdentifier provider</param>
        /// <param name="name">MarketDataIdentifier name</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>IMarketData</returns>
        public static async Task<IMarketData> GetMarketDataReferenceAsync(this IMetadataService metadataService, string provider, string name, CancellationToken ctk = default)
        {
            var entity = await metadataService.ReadMarketDataRegistryAsync(new MarketDataIdentifier(provider, name), ctk);

            if (entity == null)
                return new MarketData(metadataService, new MarketDataIdentifier(provider, name));

            switch (entity.Type)
            {
                case MarketDataType.ActualTimeSerie:
                    {
                        var actual = new ActualTimeSerie(metadataService, entity);
                        return actual;
                    }
                case MarketDataType.MarketAssessment:
                    {
                        var marketAssessment = new MarketAssessment(metadataService, entity);
                        return marketAssessment;
                    }
                case MarketDataType.VersionedTimeSerie:
                    {
                        var versioned = new VersionedTimeSerie(metadataService, entity);
                        return versioned;
                    }
                default:
                    throw new NotSupportedException($"The Type '{entity.Type}'is not present");
            }
        }
    }
}
