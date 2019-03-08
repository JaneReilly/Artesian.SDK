using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artesian.SDK.Service
{
    public class MasQueryParamaters : QueryParamaters, IQueryPartition<MasQueryParamaters>
    {
        protected IEnumerable<string> products;
       
        public MasQueryParamaters(IEnumerable<int> ids , IEnumerable<string> products)
        {
            this.ids = ids;
            this.products = products;
        }

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
