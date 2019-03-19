// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Partition Strategy
    /// </summary>
    public interface IPartitionStrategy
    {
        /// <summary>
        /// Actual Partition
        /// </summary>
        /// <param name="paramaters"></param>
        /// <returns></returns>
        IEnumerable<ActualQueryParamaters> Partition(IEnumerable<ActualQueryParamaters> paramaters);
        /// <summary>
        /// Versioned Partition
        /// </summary>
        /// <param name="paramaters"></param>
        /// <returns></returns>
        IEnumerable<VersionedQueryParamaters> Partition(IEnumerable<VersionedQueryParamaters> paramaters);
        /// <summary>
        /// Mas Partition
        /// </summary>
        /// <param name="paramaters"></param>
        /// <returns></returns>
        IEnumerable<MasQueryParamaters> Partition(IEnumerable<MasQueryParamaters> paramaters);

    }
}
