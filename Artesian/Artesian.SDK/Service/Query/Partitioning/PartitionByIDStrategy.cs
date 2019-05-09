// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;
using System.Linq;
namespace Artesian.SDK.Service
{
    /// <summary>
    /// Strategy to partition Query Paramaters by MarketData ID
    /// </summary>
    public class PartitionByIDStrategy : IPartitionStrategy
    {
        private const int _partitionSize = 25;

        /// <summary>
        /// Partition strategy for Actual Time Serie Query
        /// </summary>
        /// <param name="queryParamaters">The list of Actual Time Serie Query paramaters to be partitioned. See <see cref="ActualQueryParamaters"/></param>
        /// <returns>
        /// The input list of Actual Time Serie Query paramaters partitioned by MarketData ID. See <see cref="ActualQueryParamaters"/>
        /// </returns>
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
                                queryParamater.TransformId,
                                queryParamater.FillerKind,
                                queryParamater.FillerDV,
                                queryParamater.FillerPeriod
                                )));
                    
        }
        /// <summary>
        /// Partition strategy for Versioned Time Serie Query
        /// </summary>
        /// <param name="queryParamaters">The list of Versioned Time Serie Query paramaters to be partitioned. See <see cref="VersionedQueryParamaters"/></param>
        /// <returns>
        /// The input list of Versioned Time Serie Query paramaters partitioned by MarketData ID. See <see cref="VersionedQueryParamaters"/>
        /// </returns>
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
                                queryParamater.VersionSelectionType,
                                queryParamater.VersionLimit,
                                queryParamater.FillerKind,
                                queryParamater.FillerDV,
                                queryParamater.FillerPeriod
                                )));
        }
        /// <summary>
        /// Partition strategy for Market Assessment Query
        /// </summary>
        /// <param name="queryParamaters">The list of Market Assessment Query paramaters to be partitioned. See <see cref="MasQueryParamaters"/></param>
        /// <returns>
        /// The input list of Market Assessment Query paramaters partitioned by MarketData ID. See <see cref="MasQueryParamaters"/>
        /// </returns>
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
                                queryParamater.Products,
                                queryParamater.FillerKind,
                                queryParamater.FillerDVs,
                                queryParamater.FillerDVo,
                                queryParamater.FillerDVc,
                                queryParamater.FillerDVh,
                                queryParamater.FillerDVl,
                                queryParamater.FillerDVvp,
                                queryParamater.FillerDVvg,
                                queryParamater.FillerDVvt,
                                queryParamater.FillerPeriod
                                )));
        }

        private IEnumerable<IEnumerable<int>> _partitionIds(IEnumerable<int> ids)
        {
            return ids.Select((x, i) => (value: x, index: i))
                .GroupBy(x => (x.index / _partitionSize))
                .Select(g => g.Select(x => x.value));
        }
    }
   
}
