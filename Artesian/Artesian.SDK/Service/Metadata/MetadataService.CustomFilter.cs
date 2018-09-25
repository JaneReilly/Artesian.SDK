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
        /// Create a new Filter
        /// </summary>
        /// <param name="filter">the entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task<CustomFilter> CreateFilter(CustomFilter filter, CancellationToken ctk = default)
        {
            filter.Validate();
            var url = "/filter";

            return _client.Exec<CustomFilter, CustomFilter>(HttpMethod.Post, url, filter);
        }
        /// <summary>
        /// Update specific Filter
        /// </summary>
        /// <param name="filterId">the entity id</param>
        /// <param name="filter">the entity we are going to update</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task<CustomFilter> UpdateFilter(int filterId, CustomFilter filter, CancellationToken ctk = default)
        {
            filter.Validate();
            var url = "/filter".AppendPathSegment(filterId);

            return _client.Exec<CustomFilter, CustomFilter>(HttpMethod.Put, url, filter);
        }
        /// <summary>
        /// Read specific filter
        /// </summary>
        /// <param name="filterId">the entity id to get</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task<CustomFilter> ReadFilter(int filterId, CancellationToken ctk = default)
        {
            var url = "/filter".AppendPathSegment(filterId);

            return _client.Exec<CustomFilter, CustomFilter>(HttpMethod.Get, url);
        }
        /// <summary>
        /// Remove specific Filter
        /// </summary>
        /// <param name="filterId">the entity id to be removed</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task<CustomFilter> RemoveFilter(int filterId, CancellationToken ctk = default)
        {
            var url = "/filter".AppendPathSegment(filterId);

            return _client.Exec<CustomFilter, CustomFilter>(HttpMethod.Delete, url);
        }
        /// <summary>
        /// Read all filters
        /// </summary>
        /// <param name="page">int</param>
        /// <param name="pageSize">int</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task<PagedResult<CustomFilter>> ReadFilters(int page, int pageSize, CancellationToken ctk = default)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and Page number need to be greater than 0. Page:" + page + " Page Size:" + pageSize);

            var url = "/filter"
                .SetQueryParam("pageSize", pageSize)
                .SetQueryParam("page", page)
                ;

            return _client.Exec<PagedResult<CustomFilter>>(HttpMethod.Get, url);
        }
    }
}
