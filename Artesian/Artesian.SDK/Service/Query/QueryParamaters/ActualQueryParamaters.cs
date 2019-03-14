// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Actual Query Paramaters DTO
    /// </summary>
    public class ActualQueryParamaters : QueryParamaters
    {
        /// <summary>
        /// Granularity
        /// </summary>
        public Granularity? Granularity;
        /// <summary>
        /// Time range
        /// </summary>
        public int? Tr;
        /// <summary>
        /// Actual Query Paramaters
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="extractionRangeSelectionConfig"></param>
        /// <param name="extractionRangeType"></param>
        /// <param name="granularity"></param>
        /// <param name="tr"></param>
        public ActualQueryParamaters(IEnumerable<int> ids, ExtractionRangeSelectionConfig extractionRangeSelectionConfig, ExtractionRangeType? extractionRangeType,  Granularity? granularity, int? tr)
        {
            this.Ids = ids;
            this.ExtractionRangeCfg = extractionRangeSelectionConfig;
            this.ExtractionRangeType = extractionRangeType;
            this.Granularity = granularity;
            this.Tr = tr;
        }
     
    }
}
