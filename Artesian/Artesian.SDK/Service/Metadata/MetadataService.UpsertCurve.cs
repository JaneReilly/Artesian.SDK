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
        /// Upsert the curve data supplied in <paramref name="data"/>
        /// </summary>
        /// <remarks>
        /// Unified controller for saving curve data
        /// ID, TimeZone and DownloadedAt fields should always be not null
        /// - Market Data Assessment: MarketAssessment field should not be null, other fields should be null
        /// - Actual TimeSerie: Rows field should not be null, other fields should be null-
        /// - Versioned TimeSerie: Rows and Version fields should not be null, other fields should be null
        /// </remarks>
        /// <param name="data">
        /// An object that rappresent MarketDataAssessment, ActualTimeSerie or VersionedTimeSerie
        /// </param>
        /// <param name="ctk"></param>
        /// <returns></returns>
        public Task UpsertCurveDataAsync(UpsertCurveData data, CancellationToken ctk = default)
        {
            var url = "/marketdata/upsertdata";

            return _client.Exec(HttpMethod.Post, url, data, ctk: ctk);
        }
    }
}
