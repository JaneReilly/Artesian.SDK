using Artesian.SDK.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artesian.SDK.Service
{
    public class ActualQueryParamaters : QueryParamaters , IQueryPartition<ActualQueryParamaters>
    {
        protected Granularity? granularity;
        protected int? tr;

        public ActualQueryParamaters(IEnumerable<int> ids,  Granularity? granularity, int? tr)
        {
            this.ids = ids;
            this.granularity = granularity;
            this.tr = tr;
        }

        public IEnumerable<ActualQueryParamaters> Partition()
        {
            int i = 0;
            int partitionSize = 25;

            var idParams = ids.GroupBy(x => (i++ / partitionSize)).ToList();
            var param = new ActualQueryParamaters(null,null,null);
            var actualQueryParams = new List<ActualQueryParamaters>();

            for (int x = 0; x < idParams.Count(); x++)
            {
                 param = new ActualQueryParamaters(idParams[x], granularity , tr );
                actualQueryParams.Add(param);
            }

            return actualQueryParams;
        }
    }
}
