// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    public interface IQueryPartition
    {
        /// <summary>
        /// Partition queries by Market data Ids
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<IEnumerable<int>> Partition<T>(IEnumerable<int> ids);
    }
}
