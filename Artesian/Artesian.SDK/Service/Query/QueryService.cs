// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Flurl;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// QueryService class
    /// Contains query types to be created
    /// </summary>
    public class QueryService: IQueryService
    {
        private IArtesianServiceConfig _cfg;
        private ArtesianPolicyConfig _policy;
        private Client _client;
        private IPartitionStrategy _partitionStrategy = new PartitionByIDsStrategy();

        /// <summary>
        /// Query service for building a query
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        public QueryService(IArtesianServiceConfig cfg)
            : this(cfg, new ArtesianPolicyConfig())
        {
        }

        /// <summary>
        /// Query service for building a query
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        /// <param name="policy">ArtesianPolicyConfig</param>
        public QueryService(IArtesianServiceConfig cfg, ArtesianPolicyConfig policy)
        {
            _cfg = cfg;
            _policy = policy;
            _client = new Client(cfg, ArtesianConstants.QueryRoute.AppendPathSegment(ArtesianConstants.QueryVersion), _policy);
        }

        /// <summary>
        /// Create Actual Time Serie Query
        /// </summary>
        /// <returns>
        /// Actual Time Serie <see cref="ActualQuery"/>
        /// </returns>
        public ActualQuery CreateActual()
        {
            return new ActualQuery(_client, _partitionStrategy);
        }

        /// <summary>
        /// Create  Actual Time Serie Query
        /// </summary>
        /// <param name="partitionStrategy"></param>
        /// <returns></returns>
        public ActualQuery CreateActual(IPartitionStrategy partitionStrategy)
        {
            return new ActualQuery(_client, partitionStrategy ?? _partitionStrategy);
        }

        /// <summary>
        /// Create Versioned Time Serie Query
        /// </summary>
        /// <returns>
        /// Versioned Time Serie <see cref="VersionedQuery"/>
        /// </returns>
        public VersionedQuery CreateVersioned()
        {
            return new VersionedQuery(_client, _partitionStrategy);
        }

        /// <summary>
        /// Create Versioned Time Serie Query
        /// </summary>
        /// <param name="partitionStrategy"></param>
        /// <returns></returns>
        public VersionedQuery CreateVersioned(IPartitionStrategy partitionStrategy)
        {
            return new VersionedQuery(_client, partitionStrategy ?? _partitionStrategy);
        }

        /// <summary>
        /// Create Market Assessment Time Serie Query
        /// </summary>
        /// <returns>
        /// Market Assessment Time Serie <see cref="MasQuery"/>
        /// </returns>
        public MasQuery CreateMarketAssessment()
        {
            return new MasQuery(_client, _partitionStrategy);
        }

        /// <summary>
        /// Create Market Assessment Time Serie Query
        /// </summary>
        /// <param name="partitionStrategy"></param>
        /// <returns></returns>
        public MasQuery CreateMarketAssessment(IPartitionStrategy partitionStrategy)
        {
            return new MasQuery(_client, partitionStrategy ?? _partitionStrategy);
        }
    }
}