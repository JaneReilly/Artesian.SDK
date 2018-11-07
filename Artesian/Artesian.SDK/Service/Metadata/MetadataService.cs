// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Flurl;

namespace Artesian.SDK.Service
{
    public partial class MetadataService : IMetadataService
    {
        private IArtesianServiceConfig _cfg;
        private static Client _client;
        /// <summary>
        /// Metadata service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        public MetadataService(IArtesianServiceConfig cfg)
        {
            _cfg = cfg;
            _client = new Client(cfg, ArtesianConstants.MetadataVersion);
        }
    }
}
