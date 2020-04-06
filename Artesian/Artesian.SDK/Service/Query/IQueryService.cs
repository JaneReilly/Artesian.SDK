// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
namespace Artesian.SDK.Service
{
    /// <summary>
    /// Query Service Interface
    /// </summary>
    public interface IQueryService
    {
        /// <summary>
        /// Create Actual Time Serie
        /// </summary>
        /// <returns></returns>
        ActualQuery CreateActual();
        /// <summary>
        /// Create Versioned Time Serie
        /// </summary>
        /// <returns></returns>
        VersionedQuery CreateVersioned();
        /// <summary>
        /// Create Market Assessment Time Serie
        /// </summary>
        /// <returns></returns>
        MasQuery CreateMarketAssessment();
        /// <summary>
        /// Create Auction Time Serie
        /// </summary>
        /// <returns></returns>
        AuctionQuery CreateAuction();
    }
}