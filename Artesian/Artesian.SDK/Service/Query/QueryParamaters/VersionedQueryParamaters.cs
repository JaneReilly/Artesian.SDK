using Artesian.SDK.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artesian.SDK.Service
{
    public class VersionedQueryParamaters : QueryParamaters, IQueryPartition<VersionedQueryParamaters>
    {
        protected VersionSelectionConfig versionSelectionConfig;
        protected VersionSelectionType? versionSelectionType;
        protected Granularity? granularity;

        public VersionedQueryParamaters(IEnumerable<int> ids, VersionSelectionConfig versionSelectionConfig, VersionSelectionType? versionSelectionType, Granularity? granularity)
        {
            this.ids = ids;
            this.versionSelectionConfig = versionSelectionConfig;
            this.versionSelectionType = versionSelectionType;
            this.granularity = granularity;
        }

        public IEnumerable<VersionedQueryParamaters> Partition()
        {
            int i = 0;
            int partitionSize = 25;

            var idParams = ids.GroupBy(x => (i++ / partitionSize)).ToList();
            var param = new VersionedQueryParamaters(null, null, null, null);
            var actualQueryParams = new List<VersionedQueryParamaters>();

            for (int x = 0; x < idParams.Count(); x++)
            {
                param = new VersionedQueryParamaters(idParams[x], versionSelectionConfig, versionSelectionType, granularity);
                actualQueryParams.Add(param);
            }

            return actualQueryParams;
        }
    
    }
}
