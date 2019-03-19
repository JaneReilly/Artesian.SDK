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
        /// <param name="queryParamaters"></param>
        /// <returns></returns>
        public IEnumerable<ActualQueryParamaters> Partition(IEnumerable<ActualQueryParamaters> queryParamaters)
        {
            
            if (queryParamaters.Any(g => g.Ids == null)) return queryParamaters;

            return queryParamaters.SelectMany(queryParamater =>
                        _partitionIds(queryParamater.Ids)
                            .Select(g => new ActualQueryParamaters(g,
                                queryParamater.ExtractionRangeSelectionConfig,
                                queryParamater.ExtractionRangeType,
                                queryParamater.TimeZone,
                                queryParamater.FilterId,
                                queryParamater.Granularity,
                                queryParamater.TransformId
                                )));
                    
        }
        /// <summary>
        /// Versioned Partition
        /// </summary>
        /// <param name="queryParamaters"></param>
        /// <returns></returns>
        public IEnumerable<VersionedQueryParamaters> Partition(IEnumerable<VersionedQueryParamaters> queryParamaters)
        {
            if (queryParamaters.Any(g => g.Ids == null)) return queryParamaters;

            return queryParamaters.SelectMany(queryParamater => 
                         _partitionIds(queryParamater.Ids)
                            .Select(g => new VersionedQueryParamaters(g,
                                queryParamater.ExtractionRangeSelectionConfig,
                                queryParamater.ExtractionRangeType,
                                queryParamater.TimeZone,
                                queryParamater.FilterId,
                                queryParamater.Granularity,
                                queryParamater.TransformId,
                                queryParamater.VersionSelectionConfig,
                                queryParamater.VersionSelectionType
                                )));
        }
        /// <summary>
        /// Mas Partition
        /// </summary>
        /// <param name="queryParamaters"></param>
        /// <returns></returns>
        public IEnumerable<MasQueryParamaters> Partition(IEnumerable<MasQueryParamaters> queryParamaters)
        {
            if (queryParamaters.Any(g => g.Ids == null)) return queryParamaters;

            return queryParamaters.SelectMany(queryParamater => 
                        _partitionIds(queryParamater.Ids)
                            .Select(g => new MasQueryParamaters(g,
                                queryParamater.ExtractionRangeSelectionConfig,
                                queryParamater.ExtractionRangeType,
                                queryParamater.TimeZone,
                                queryParamater.FilterId,
                                queryParamater.Products
                                )));
        }
        private IEnumerable<IEnumerable<int>> _partitionIds(IEnumerable<int> ids)
        {
            return ids.Select((x, i) => (value: x, index: i))
                .GroupBy(x => (x.index / PartitionSize))
                .Select(g => g.Select(x => x.value));
        }
    }
   
}
