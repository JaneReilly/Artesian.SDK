// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;
using System.Linq;

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
        public MasQueryParamaters(
            IEnumerable<int> ids , 
            ExtractionRangeSelectionConfig extractionRangeSelectionConfig, 
            ExtractionRangeType? extractionRangeType,
            string timezone,
            int? filterId,
            IEnumerable<string> products)
            : base(ids, extractionRangeSelectionConfig, extractionRangeType, timezone, filterId)
        {
            this.Products = products;
        }
        /// <summary>
        /// Products
        /// </summary>
        public IEnumerable<string> Products { get; set; }
    }
}
