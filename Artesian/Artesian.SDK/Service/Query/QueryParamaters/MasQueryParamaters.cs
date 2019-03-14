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
        /// Products
        /// </summary>
       public IEnumerable<string> Products;
        /// <summary>
        /// Mas Query Paramaters
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="extractionRangeSelectionConfig"></param>
        /// <param name="extractionRangeType"></param>
        /// <param name="products"></param>
        public MasQueryParamaters(IEnumerable<int> ids , ExtractionRangeSelectionConfig extractionRangeSelectionConfig, ExtractionRangeType? extractionRangeType, IEnumerable<string> products)
        {
            this.Ids = ids;
            this.ExtractionRangeCfg = extractionRangeSelectionConfig;
            this.ExtractionRangeType = extractionRangeType;
            this.Products = products;
        }
        /// <summary>
        /// Partition Mas Query by ID's
        /// </summary>
        /// <returns>List of Mas Queries Partitioned by ID</returns>
        public IEnumerable<MasQueryParamaters> Partition()
        {
            int i = 0;
            int partitionSize = 25;

            var idParams = Ids.GroupBy(x => (i++ / partitionSize)).ToList();
            var param = new MasQueryParamaters(null, new ExtractionRangeSelectionConfig(), null,null);
            var actualQueryParams = new List<MasQueryParamaters>();

            for (int x = 0; x < idParams.Count(); x++)
            {
                param = new MasQueryParamaters(idParams[x], ExtractionRangeCfg, ExtractionRangeType, Products);
                actualQueryParams.Add(param);
            }

            return actualQueryParams;
        }
    }
}
