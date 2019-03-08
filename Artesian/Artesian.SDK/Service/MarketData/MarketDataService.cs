// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 

using Flurl;
using System;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// MarketData service
    /// </summary>
    public partial class MarketDataService : IMarketDataService
    {
        private IArtesianServiceConfig _cfg;
        private ArtesianPolicyConfig _policy;
        private static Client _client;
        /// <summary>
        /// Metadata service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        public MarketDataService(IArtesianServiceConfig cfg)
        {
            _cfg = cfg;
            _policy = new ArtesianPolicyConfig();
            _client = new Client(cfg, ArtesianConstants.MetadataVersion, _policy);
        }
        /// <summary>
        /// Metadata service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        /// <param name="policy">ArtesianPolicyConfig</param>
        public MarketDataService(IArtesianServiceConfig cfg, ArtesianPolicyConfig policy)
        {
            _cfg = cfg;
            _client = new Client(cfg, ArtesianConstants.MetadataVersion, policy);
        }
    }
}
