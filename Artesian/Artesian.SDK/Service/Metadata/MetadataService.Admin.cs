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
        /// Create a new Authorization Group
        /// </summary>
        /// <param name="group">the entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task<AuthGroup> CreateAuthGroup(AuthGroup group, CancellationToken ctk = default)
        {
            var url = "/group";

            return _client.Exec<AuthGroup, AuthGroup>(HttpMethod.Post, url, group);
        }
        /// <summary>
        /// Update an Authorization Group
        /// </summary>
        /// <param name="groupID">the entity Identifier</param>
        /// <param name="group">the entity to update</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task<AuthGroup> UpdateAuthGroup(int groupID, AuthGroup group, CancellationToken ctk = default)
        {
            var url = "/group".AppendPathSegment(groupID);

            return _client.Exec<AuthGroup, AuthGroup>(HttpMethod.Put, url, group);
        }
        /// <summary>
        /// Remove an Authorization Group
        /// </summary>
        /// <param name="groupID">the entity Identifier</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public Task RemoveAuthGroup(int groupID, CancellationToken ctk = default)
        {
            var url = "/group".AppendPathSegment(groupID);

            return _client.Exec(HttpMethod.Delete, url);
        }
        /// <summary>
        /// Read Authorization Group
        /// </summary>
        /// <param name="groupID">the entity Identifier</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task<AuthGroup> ReadAuthGroup(int groupID, CancellationToken ctk = default)
        {
            var url = "/group".AppendPathSegment(groupID);

            return _client.Exec<AuthGroup>(HttpMethod.Get, url);
        }
        /// <summary>
        /// Remove an Authorization Group
        /// </summary>
        /// <param name="page">the requested page</param>
        /// <param name="pageSize">the size of the page</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public Task<PagedResult<AuthGroup>> ReadAuthGroups(int page, int pageSize, CancellationToken ctk = default)
        {
            var url = "/group"
                    .SetQueryParam("pageSize", pageSize)
                    .SetQueryParam("page", page);

            return _client.Exec<PagedResult<AuthGroup>>(HttpMethod.Get, url);
        }
        /// <summary>
        /// Get a list of Principals of hte selected user
        /// </summary>
        /// <param name="user">the user name</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public Task<List<Principals>> ReadUserPrincipals(string user, CancellationToken ctk = default)
        {
            var url = "/user/principals"
                        .SetQueryParam("user", $"{user}");

            return _client.Exec<List<Principals>>(HttpMethod.Get, url);
        }
    }
}
