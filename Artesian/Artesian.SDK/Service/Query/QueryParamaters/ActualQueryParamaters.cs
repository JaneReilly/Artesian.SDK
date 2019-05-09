// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using NodaTime;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Actual Query Paramaters DTO
    /// </summary>
    public class ActualQueryParamaters : QueryParamaters
    {
        /// <summary>
        /// 
        /// </summary>
        public ActualQueryParamaters()
        {

        }

        /// <summary>
        /// Actual Query Paramaters
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="extractionRangeSelectionConfig"></param>
        /// <param name="extractionRangeType"></param>
        /// <param name="granularity"></param>
        /// <param name="transformId"></param>
        /// <param name="timezone"></param>
        /// <param name="filterId"></param>
        /// <param name="fillerK"></param>
        /// <param name="fillerDV"></param>
        /// <param name="fillerP"></param>
        public ActualQueryParamaters(
            IEnumerable<int> ids, 
            ExtractionRangeSelectionConfig extractionRangeSelectionConfig, 
            ExtractionRangeType? extractionRangeType,
            string timezone,
            int? filterId,
            Granularity? granularity, 
            int? transformId,
            FillerKind fillerK,
            double? fillerDV,
            Period fillerP
            )
            : base(ids,extractionRangeSelectionConfig, extractionRangeType, timezone, filterId, fillerK)
        {           
            this.Granularity = granularity;
            this.TransformId = transformId;
            this.FillerDV = fillerDV;
            this.FillerPeriod = fillerP;
        }

        /// <summary>
        /// Granularity
        /// </summary>
        public Granularity? Granularity { get; set; }
        /// <summary>
        /// Time range
        /// </summary>
        public int? TransformId { get; set; }
        /// <summary>
        /// Filler Default Value
        /// </summary>
        public double? FillerDV { get; set; }
        /// <summary>
        /// Filler Period
        /// </summary>
        public Period FillerPeriod { get; set; }
    }
}
