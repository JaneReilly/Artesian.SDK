// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Mas Query Paramaters DTO
    /// </summary>
    public class MasQueryParamaters : QueryParamaters
    {    
        /// <summary>
        /// 
        /// </summary>
        public MasQueryParamaters()
        {

        }

        /// <summary>
        /// Mas Query Paramaters
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="extractionRangeSelectionConfig"></param>
        /// <param name="extractionRangeType"></param>
        /// <param name="timezone"></param>
        /// <param name="filterId"></param>
        /// <param name="products"></param>
        /// <param name="fillerK"></param>
        /// <param name="fillerDVs"></param>
        /// <param name="fillerDVo"></param>
        /// <param name="fillerDVc"></param>
        /// <param name="fillerDVh"></param>
        /// <param name="fillerDVl"></param>
        /// <param name="fillerDVvp"></param>
        /// <param name="fillerDVvg"></param>
        /// <param name="fillerDVvt"></param>
        /// <param name="fillerP"></param>
        public MasQueryParamaters(
            IEnumerable<int> ids , 
            ExtractionRangeSelectionConfig extractionRangeSelectionConfig, 
            ExtractionRangeType? extractionRangeType,
            string timezone,
            int? filterId,
            IEnumerable<string> products,
            FillerKind fillerK,
            double? fillerDVs,
            double? fillerDVo,
            double? fillerDVc,
            double? fillerDVh,
            double? fillerDVl,
            double? fillerDVvp,
            double? fillerDVvg,
            double? fillerDVvt,
            Period fillerP
            )
            : base(ids, extractionRangeSelectionConfig, extractionRangeType, timezone, filterId, fillerK)
        {
            this.Products = products;
            this.FillerDVs = fillerDVs;
            this.FillerDVo = fillerDVo;
            this.FillerDVc = fillerDVc;
            this.FillerDVh = fillerDVh;
            this.FillerDVl = fillerDVl;
            this.FillerDVvp = fillerDVvp;
            this.FillerDVvg = fillerDVvg;
            this.FillerDVvt = fillerDVvt;
            this.FillerPeriod = fillerP;
        }
        /// <summary>
        /// Products
        /// </summary>
        public IEnumerable<string> Products { get; set; }
        /// <summary>
        /// Filler Default Value Settlement
        /// </summary>
        public double? FillerDVs { get; set; }
        /// <summary>
        /// Filler Default Value Open
        /// </summary>
        public double? FillerDVo { get; set; }
        /// <summary>
        /// Filler Default Value Close
        /// </summary>
        public double? FillerDVc { get; set; }
        /// <summary>
        /// Filler Default Value High
        /// </summary>
        public double? FillerDVh { get; set; }
        /// <summary>
        /// Filler Default Value Low
        /// </summary>
        public double? FillerDVl { get; set; }
        /// <summary>
        /// Filler Default Value Volume Paid
        /// </summary>
        public double? FillerDVvp { get; set; }
        /// <summary>
        /// Filler Default Value Volume Given
        /// </summary>
        public double? FillerDVvg { get; set; }
        /// <summary>
        /// Filler Default Value Volume Total
        /// </summary>
        public double? FillerDVvt { get; set; }
        /// <summary>
        /// Filler Period
        /// </summary>
        public Period FillerPeriod { get; set; }
    }
}
