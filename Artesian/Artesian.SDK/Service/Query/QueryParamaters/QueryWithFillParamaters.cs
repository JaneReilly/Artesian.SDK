// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Query Paramaters DTO
    /// </summary>
    public abstract class QueryWithFillParamaters: BaseQueryParamaters
    {
        /// <summary>
        /// 
        /// </summary>
        public QueryWithFillParamaters()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="extractionRangeSelectionConfig"></param>
        /// <param name="extractionRangeType"></param>
        /// <param name="timezone"></param>
        /// <param name="filterId"></param>
        /// <param name="fillerKind"></param>
        /// <param name="fillerConfig"></param>
        public QueryWithFillParamaters(
            IEnumerable<int> ids, 
            ExtractionRangeSelectionConfig 
            extractionRangeSelectionConfig, 
            ExtractionRangeType? 
            extractionRangeType, 
            string timezone, 
            int? filterId,
            FillerKindType fillerKind,
            FillerConfig fillerConfig
            ): base(ids, extractionRangeSelectionConfig, extractionRangeType, timezone, filterId)
        {
            this.FillerKindType = fillerKind;
            this.FillerConfig = fillerConfig;
        }

        /// <summary>
        /// Filler Kind
        /// </summary>
        public FillerKindType FillerKindType { get; set; } = FillerKindType.Default;
        /// <summary>
        /// Filler config
        /// </summary>
        public FillerConfig FillerConfig { get; set; } = new FillerConfig();
    }
}
