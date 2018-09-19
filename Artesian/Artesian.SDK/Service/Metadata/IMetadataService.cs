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
    }
}
