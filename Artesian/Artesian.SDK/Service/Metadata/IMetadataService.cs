// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using NodaTime;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Metadata service interface
    /// </summary>
    public interface IMetadataService
    {
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
        /// Search the market data collection with faceted results
        /// </summary>
        /// <param name="filter">ArtesianSearchFilter</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task<ArtesianSearchResults> SearchFacetAsync(ArtesianSearchFilter filter, CancellationToken ctk = default(CancellationToken));
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
    }
}
