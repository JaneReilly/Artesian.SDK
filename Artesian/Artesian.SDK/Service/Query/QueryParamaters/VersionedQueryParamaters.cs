// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Versioned Query Paramaters DTO
    /// </summary>
    public class VersionedQueryParamaters : QueryParamaters
    {
        /// <summary>
        /// Version selection config
        /// </summary>
        public VersionSelectionConfig VersionSelectionConfig;
        /// <summary>
        /// Version selection type
        /// </summary>
        public VersionSelectionType? VersionSelectionType;
        /// <summary>
        /// Granularity
        /// </summary>
        public Granularity? Granularity;
        /// <summary>
        /// Time range
        /// </summary>
        public int? Tr;
        /// <summary>
        /// Versioned Query Paramters
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="extractionRangeSelectionConfig"></param>
        /// <param name="extractionRangeType"></param>
        /// <param name="versionSelectionConfig"></param>
        /// <param name="versionSelectionType"></param>
        /// <param name="granularity"></param>
        /// <param name="tr"></param>
        public VersionedQueryParamaters(IEnumerable<int> ids, ExtractionRangeSelectionConfig extractionRangeSelectionConfig, ExtractionRangeType? extractionRangeType, VersionSelectionConfig versionSelectionConfig, VersionSelectionType? versionSelectionType, Granularity? granularity, int? tr)
        {
            this.Ids = ids;
            this.ExtractionRangeCfg = extractionRangeSelectionConfig;
            this.ExtractionRangeType = extractionRangeType;
            this.VersionSelectionConfig = versionSelectionConfig;
            this.VersionSelectionType = versionSelectionType;
            this.Granularity = granularity;
            this.Tr = tr;
        }
    }
}
