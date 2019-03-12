// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;
using System.Linq;
namespace Artesian.SDK.Service
{
    /// <summary>
    /// Partition by ID's Strategy
    /// </summary>
    public class PartitionByIDsStrategy : IPartitionStrategy
    {
        const int PartitionSize = 25;
        /// <summary>
        /// Actual Partition
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public IEnumerable<ActualQueryParamaters> Partition(IEnumerable<ActualQueryParamaters> queries)
        {

            return queries.SelectMany(query =>
                        _partitionIds(query.ids)
                        .Select(g => new ActualQueryParamaters(g, query.granularity, query.tr))
                    );
        }
        /// <summary>
        /// Versioned Partition
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public IEnumerable<VersionedQueryParamaters> Partition(IEnumerable<VersionedQueryParamaters> queries)
        {

            return queries.SelectMany(query =>
                        _partitionIds(query.ids)
                        .Select(g => new VersionedQueryParamaters(g, query.versionSelectionConfig, query.versionSelectionType, query.granularity, query.tr))
                    );
        }
        /// <summary>
        /// Mas Partition
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public IEnumerable<MasQueryParamaters> Partition(IEnumerable<MasQueryParamaters> queries)
        {

            return queries.SelectMany(query =>
                        _partitionIds(query.ids)
                        .Select(g => new MasQueryParamaters(g, query.products))
                    );
        }
        private IEnumerable<IEnumerable<int>> _partitionIds(IEnumerable<int> ids)
        {
            return ids.Select((x, i) => (value: x, index: i))
                .GroupBy(x => (x.index / PartitionSize))
                .Select(g => g.Select(x => x.value));
        }
    }
   
}
