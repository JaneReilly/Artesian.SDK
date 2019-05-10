// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Query Paramaters DTO
    /// </summary>
    public abstract class QueryParamaters
    {
        /// <summary>
        /// 
        /// </summary>
        public QueryParamaters()
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
        public QueryParamaters(
            IEnumerable<int> ids, 
            ExtractionRangeSelectionConfig 
            extractionRangeSelectionConfig, 
            ExtractionRangeType? 
            extractionRangeType, 
            string timezone, 
            int? filterId,
            FillerKindType fillerKind,
            FillerConfig fillerConfig
            )
        {
            this.Ids = ids;
            this.ExtractionRangeSelectionConfig = extractionRangeSelectionConfig;
            this.ExtractionRangeType = extractionRangeType;
            this.TimeZone = timezone;
            this.FilterId = filterId;
            this.FillerKindType = fillerKind;
            this.FillerConfig = fillerConfig;
        }

        /// <summary>
        /// IDs
        /// </summary>
        public IEnumerable<int> Ids { get; set; }
        /// <summary>
        /// Extraction range config
        /// </summary>
        public ExtractionRangeSelectionConfig ExtractionRangeSelectionConfig { get; set; } = new ExtractionRangeSelectionConfig();
        /// <summary>
        /// Extraction range type
        /// </summary>
        public ExtractionRangeType? ExtractionRangeType { get; set; }
        /// <summary>
        /// Timezone
        /// </summary>
        public string TimeZone { get; set; }
        /// <summary>
        /// FilterId
        /// </summary>
        public int? FilterId { get; set; }
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
