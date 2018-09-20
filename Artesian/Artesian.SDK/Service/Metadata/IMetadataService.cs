// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using NodaTime;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Metadata service interface
    /// </summary>
    public interface IMetadataService
    {
        #region MarketData
        /// <summary>
        /// Get Metadata by provider and curve name with MarketDataIdentifier
        /// </summary>
        /// <param name="id">MarketDataIdentifier</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<MarketDataEntity.Output> ReadMarketDataRegistryAsync(MarketDataIdentifier id, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Read Metadata by curve id
        /// </summary>
        /// <param name="id">An Int</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<MarketDataEntity.Output> ReadMarketDataRegistryAsync(int id, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Get the metadata versions by id
        /// </summary>
        /// <param name="id">Int</param>
        /// <param name="page">Int</param>
        /// <param name="pageSize">Int</param>
        /// <param name="product">string</param>
        /// <param name="versionFrom">LocalDateTime</param>
        /// <param name="versionTo">LocalDateTime</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<PagedResult<CurveRange>> ReadCurveRangeAsync(int id, int page, int pageSize, string product = null, LocalDateTime? versionFrom = null, LocalDateTime? versionTo = null, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Register the given MarketData entity
        /// </summary>
        /// <param name="metadata">MarketDataEntity</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task<MarketDataEntity.Output> RegisterMarketDataAsync(MarketDataEntity.Input metadata, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Save the given MarketData entity
        /// </summary>
        /// <param name="metadata">MarketDataEntity</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task<MarketDataEntity.Output> UpdateMarketDataAsync(MarketDataEntity.Input metadata, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Delete the specific MarketData entity by id
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task DeleteMarketDataAsync(int id, CancellationToken ctk = default(CancellationToken));
        #endregion

        #region SearchFacet
        /// <summary>
        /// Search the market data collection with faceted results
        /// </summary>
        /// <param name="filter">ArtesianSearchFilter</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task<ArtesianSearchResults> SearchFacetAsync(ArtesianSearchFilter filter, CancellationToken ctk = default(CancellationToken));
        #endregion

        #region Operations
        /// <summary>
        /// A sequence of operation will be applied to the metadata identified by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="operations"></param>
        /// <param name="ctk"></param>
        /// <returns></returns>
        Task<List<MarketDataEntity.Output>> PerformOperationsAsync(Operations operations, CancellationToken ctk = default);
        #endregion

        #region TimeTransform
        /// <summary>
        /// Retrieve the TimeTransform entity from the database
        /// </summary>
        /// <param name="timeTransformId">An Int</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<TimeTransform> ReadTimeTransformBaseAsync(int timeTransformId, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Read the TimeTransform entity from the database paged
        /// </summary>
        /// <param name="page">int</param>
        /// <param name="pageSize">int</param>
        /// <param name="userDefined">bool</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<PagedResult<TimeTransform>> ReadTimeTransformsAsync(int page, int pageSize, bool userDefined, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Register a new TimeTransform
        /// </summary>
        /// <param name="timeTransform">the entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<TimeTransform> RegisterTimeTransformBaseAsync(TimeTransform timeTransform, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Update the TimeTransform
        /// </summary>
        /// <param name="timeTransform">the entity we are going to update</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<TimeTransform> UpdateTimeTransformBaseAsync(TimeTransform timeTransform, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Delete the TimeTransform
        /// </summary>
        /// <param name="timeTransformId">the entity id we are going to delete</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task DeleteTimeTransformSimpleShiftAsync(int timeTransformId, CancellationToken ctk = default(CancellationToken));
        #endregion

        #region Filters
        /// <summary>
        /// Create a new Filter
        /// </summary>
        /// <param name="filter">the entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<CustomFilter> CreateFilter(CustomFilter filter, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Update specific Filter
        /// </summary>
        /// <param name="filterId">the entity id</param>
        /// <param name="filter">the entity we are going to update</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<CustomFilter> UpdateFilter(int filterId, CustomFilter filter, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Read specific filter
        /// </summary>
        /// <param name="filterId">the entity id to get</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<CustomFilter> ReadFilter(int filterId, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Remove specific Filter
        /// </summary>
        /// <param name="filterId">the entity id to be removed</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<CustomFilter> RemoveFilter(int filterId, CancellationToken ctk = default(CancellationToken));
        /// <summary>
        /// Read all filters
        /// </summary>
        /// <param name="page">int</param>
        /// <param name="pageSize">int</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<PagedResult<CustomFilter>> ReadFilters(int page, int pageSize, CancellationToken ctk = default(CancellationToken));
        #endregion

        #region Acl
        /// <summary>
        /// Retrieve the ACL Path Roles by path
        /// </summary>
        /// <param name="path">The path (starting with "/" char. Ex. "/marketdata/system/" identifies folder "marketdata" with a subfolder "system", roles are assigned to "system" subfolder. Ex. "/marketdata/genoacurve" identifies folder "marketdata" with entity "genoacurve", roles are assigned to "genoacurve" entity.</param>
        /// <param name="ctk"></param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<IEnumerable<AuthorizationPath.Output>> ReadRolesByPath(PathString path, CancellationToken ctk = default);
        /// <summary>
        /// Retrieve the ACL Path Roles paged
        /// </summary>
        /// <param name="page">the requested page</param>
        /// <param name="pageSize">the size of the page</param>
        /// <param name="principalIds">The principal ids I want to inspect, encoded.( ex. u:user@example.com for users and clients,g:1001 for groups)</param>
        /// <param name="asOf">LocalDateTime we want to inspect</param>
        /// <param name="ctk"></param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<PagedResult<AuthorizationPath.Output>> GetRoles(int page, int pageSize, string[] principalIds, LocalDateTime? asOf = null, CancellationToken ctk = default);
        /// <summary>
        /// Upsert the ACL Path Roles
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="upsert">the entity we want to upsert</param>
        /// <param name="ctk"></param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task UpsertRoles(AuthorizationPath.Input upsert, CancellationToken ctk = default);
        /// <summary>
        /// Add a role to the ACL Path
        /// </summary>
        /// <param name="add">the entity we want to add. At the path add.Path we add the add.Roles</param>
        /// <param name="ctk"></param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task AddRoles(AuthorizationPath.Input add, CancellationToken ctk = default);
        /// <summary>
        /// Remove a role from the ACL Path
        /// </summary>
        /// <param name="remove">the entity we want to remove. At the path remove.Path we remove the remove.Roles</param>
        /// <param name="ctk"></param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task RemoveRoles(AuthorizationPath.Input remove, CancellationToken ctk = default);
        #endregion

        #region Admin
        /// <summary>
        /// Create a new Authorization Group
        /// </summary>
        /// <param name="group">the entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<AuthGroup> CreateAuthGroup(AuthGroup group, CancellationToken ctk = default);
        /// <summary>
        /// Update an Authorization Group
        /// </summary>
        /// <param name="groupID">the entity Identifier</param>
        /// <param name="group">the entity to update</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<AuthGroup> UpdateAuthGroup(int groupID, AuthGroup group, CancellationToken ctk = default);
        /// <summary>
        /// Remove an Authorization Group
        /// </summary>
        /// <param name="groupID">the entity Identifier</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task RemoveAuthGroup(int groupID, CancellationToken ctk = default);
        /// <summary>
        /// Read Authorization Group
        /// </summary>
        /// <param name="groupID">the entity Identifier</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        Task<AuthGroup> ReadAuthGroup(int groupID, CancellationToken ctk = default);
        /// <summary>
        /// Remove an Authorization Group
        /// </summary>
        /// <param name="page">the requested page</param>
        /// <param name="pageSize">the size of the page</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task<PagedResult<AuthGroup>> ReadAuthGroups(int page, int pageSize, CancellationToken ctk = default);
        /// <summary>
        /// Get a list of Principals of hte selected user
        /// </summary>
        /// <param name="user">the user name</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task<List<Principals>> ReadUserPrincipals(string user, CancellationToken ctk = default);
        #endregion

        #region UpsertCurve
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
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task UpsertCurveDataAsync(UpsertCurveData data, CancellationToken ctk = default);
        #endregion

        #region ApiKey
        /// <summary>
        /// Create new ApiKey
        /// </summary>
        /// <param name="apiKeyRecord">the entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task<ApiKey.Output> CreateApiKeyAsync(ApiKey.Input apiKeyRecord, CancellationToken ctk = default);
        /// <summary>
        /// Retrieve the ApiKey entity
        /// </summary>
        /// <param name="key">The Key</param>
        /// <returns></returns>
        Task<ApiKey.Output> ReadApiKeyByKeyAsync(string key, CancellationToken ctk = default);
        /// <summary>
        /// Retrieve the ApiKey entity
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task<ApiKey.Output> ReadApiKeyByIdAsync(int id, CancellationToken ctk = default);
        /// <summary>
        /// Retrieve the apikeys paged
        /// </summary>
        /// <param name="page">the requested page</param>
        /// <param name="pageSize">the size of the page</param>
        /// <param name="userId">the userid we want to filter for</param>y</param>
        /// <returns></returns>
        Task<PagedResult<ApiKey.Output>> ReadApiKeysAsync(int page, int pageSize, string userId, CancellationToken ctk = default);
        /// <summary>
        /// Delete the ApiKey
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task DeleteApiKeyAsync(int id, CancellationToken ctk = default);
        #endregion
    }
}
