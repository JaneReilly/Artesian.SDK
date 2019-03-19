﻿// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Strategy to partition service requests
    /// </summary>
    public interface IPartitionStrategy
    {
        /// <summary>
        /// Partition strategy for Actual Time Serie Query
        /// </summary>
        /// <param name="paramaters">The list of Actual Time Serie Query paramaters to be partitioned</param>
        /// <returns>The input list of Actual Time Serie Query paramaters partitioned with the defined strategy</returns>
        IEnumerable<ActualQueryParamaters> Partition(IEnumerable<ActualQueryParamaters> paramaters);
        /// <summary>
        /// Partition strategy for Versioned Time Serie Query
        /// </summary>
        /// <param name="paramaters">The list of Versioned Time Serie Query paramaters to be partitioned</param>
        /// <returns>The input list of Versioned Time Serie Query paramaters partitioned with the defined strategy</returns>
        IEnumerable<VersionedQueryParamaters> Partition(IEnumerable<VersionedQueryParamaters> paramaters);
        /// <summary>
        /// Partition strategy for Market Assessment Query
        /// </summary>
        /// <param name="paramaters">The list of Market Assessment Query paramaters to be partitioned</param>
        /// <returns>The input list of Market Assessment Query paramaters partitioned with the defined strategy</returns>
        IEnumerable<MasQueryParamaters> Partition(IEnumerable<MasQueryParamaters> paramaters);

    }
}
