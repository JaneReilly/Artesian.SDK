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
        public IEnumerable<ActualQueryParamaters> Partition(ActualQueryParamaters queryParamaters)
        {
            if (queryParamaters.Ids == null) return new[] { queryParamaters };

            return _partitionIds(queryParamaters.Ids)
                        .Select(g => new ActualQueryParamaters(g,
                            queryParamaters.ExtractionRangeSelectionConfig,
                            queryParamaters.ExtractionRangeType,
                            queryParamaters.TimeZone,
                            queryParamaters.FilterId,
                            queryParamaters.Granularity,
                            queryParamaters.TransformId
                            ));
                    
        }
        /// <summary>
        /// Versioned Partition
        /// </summary>
        /// <param name="queryParamaters"></param>
        /// <returns></returns>
        public IEnumerable<VersionedQueryParamaters> Partition(VersionedQueryParamaters queryParamaters)
        {
            if (queryParamaters.Ids == null) return new[] { queryParamaters };

            return _partitionIds(queryParamaters.Ids)
                        .Select(g => new VersionedQueryParamaters(g, 
                            queryParamaters.ExtractionRangeSelectionConfig, 
                            queryParamaters.ExtractionRangeType,
                            queryParamaters.TimeZone,
                            queryParamaters.FilterId,
                            queryParamaters.Granularity,
                            queryParamaters.TransformId,
                            queryParamaters.VersionSelectionConfig,
                            queryParamaters.VersionSelectionType
                            ));
        }
        /// <summary>
        /// Mas Partition
        /// </summary>
        /// <param name="queryParamaters"></param>
        /// <returns></returns>
        public IEnumerable<MasQueryParamaters> Partition(MasQueryParamaters queryParamaters)
        {
            if (queryParamaters.Ids == null) return new[] { queryParamaters };

            return _partitionIds(queryParamaters.Ids)
                        .Select(g => new MasQueryParamaters(g, 
                            queryParamaters.ExtractionRangeSelectionConfig,
                            queryParamaters.ExtractionRangeType,
                            queryParamaters.TimeZone,
                            queryParamaters.FilterId,
                            queryParamaters.Products))
                    ;
        }
        private IEnumerable<IEnumerable<int>> _partitionIds(IEnumerable<int> ids)
        {
            return ids.Select((x, i) => (value: x, index: i))
                .GroupBy(x => (x.index / PartitionSize))
                .Select(g => g.Select(x => x.value));
        }
    }
   
}
