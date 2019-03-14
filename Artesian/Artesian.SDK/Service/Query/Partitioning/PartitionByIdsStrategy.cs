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
        private const int PartitionSize = 25;
        /// <summary>
        /// Actual Partition
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public IEnumerable<ActualQueryParamaters> Partition(IEnumerable<ActualQueryParamaters> queries)
        {

            return queries.SelectMany(query =>
                        _partitionIds(query.Ids)
                        .Select(g => new ActualQueryParamaters(g, query.ExtractionRangeCfg, query.ExtractionRangeType, query.Granularity, query.Tr))
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
                        _partitionIds(query.Ids)
                        .Select(g => new VersionedQueryParamaters(g, query.ExtractionRangeCfg, query.ExtractionRangeType, query.VersionSelectionConfig, query.VersionSelectionType, query.Granularity, query.Tr))
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
                        _partitionIds(query.Ids)
                        .Select(g => new MasQueryParamaters(g, query.ExtractionRangeCfg, query.ExtractionRangeType, query.Products))
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
