// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artesian.SDK.Dto;
using Flurl;
using NodaTime;
using System;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    public partial class MetadataService : IMetadataService
    {
        /// <summary>
        /// A sequence of operation will be applied to the metadata identified by ids
        /// </summary>
        /// <param name="operations"></param>
        /// <param name="ctk"></param>
        /// <returns></returns>
        public Task<List<MarketDataEntity.Output>> PerformOperationsAsync(Operations operations, CancellationToken ctk = default)
        {
            var url = "/marketdata/operations";

            return _client.Exec<List<MarketDataEntity.Output>, Operations>(HttpMethod.Post, url, operations, ctk: ctk);
        }
    }
}
