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
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IEnumerable<string> products;
       /// <summary>
       /// Mas Query Paramaters
       /// </summary>
       /// <param name="ids"></param>
       /// <param name="products"></param>
        public MasQueryParamaters(IEnumerable<int> ids , IEnumerable<string> products)
        {
            this.ids = ids;
            this.products = products;
        }
        /// <summary>
        /// Partition Mas Query by ID's
        /// </summary>
        /// <returns>List of Mas Queries Partitioned by ID</returns>
        public IEnumerable<MasQueryParamaters> Partition()
        {
            int i = 0;
            int partitionSize = 25;

            var idParams = ids.GroupBy(x => (i++ / partitionSize)).ToList();
            var param = new MasQueryParamaters(null, null);
            var actualQueryParams = new List<MasQueryParamaters>();

            for (int x = 0; x < idParams.Count(); x++)
            {
                param = new MasQueryParamaters(idParams[x], products);
                actualQueryParams.Add(param);
            }

            return actualQueryParams;
        }
    }
}
