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
        /// <returns>IMarketData</returns>
        public static IMarketData GetMarketDataReference(this IMetadataService metadataService, MarketDataIdentifier id)
        {
            return new MarketData(metadataService, id);
        }

        /// <summary>
        /// Read marketdata entity by MarketDataIdentifier and returns an istance of IMarketData if exists
        /// </summary>
        /// <param name="metadataService">MarketDataIdentifier of markedata to be retrieved</param>
        /// <param name="provider">MarketDataIdentifier provider</param>
        /// <param name="name">MarketDataIdentifier name</param>
        /// <returns>IMarketData</returns>
        public static IMarketData GetMarketDataReference(this IMetadataService metadataService, string provider, string name)
        {
            return new MarketData(metadataService, new MarketDataIdentifier(provider, name));
        }
    }
}
