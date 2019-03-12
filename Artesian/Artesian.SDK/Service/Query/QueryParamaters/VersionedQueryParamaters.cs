// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Versioned Query Paramaters DTO
    /// </summary>
    public class VersionedQueryParamaters : QueryParamaters
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public VersionSelectionConfig versionSelectionConfig;
        public VersionSelectionType? versionSelectionType;
        public Granularity? granularity;
        public int? tr;
        /// <summary>
        /// Versioned Query Paramters
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="versionSelectionConfig"></param>
        /// <param name="versionSelectionType"></param>
        /// <param name="granularity"></param>
        /// <param name="tr"></param>
        public VersionedQueryParamaters(IEnumerable<int> ids, VersionSelectionConfig versionSelectionConfig, VersionSelectionType? versionSelectionType, Granularity? granularity, int? tr)
        {
            this.ids = ids;
            this.versionSelectionConfig = versionSelectionConfig;
            this.versionSelectionType = versionSelectionType;
            this.granularity = granularity;
            this.tr = tr;
        }
    }
}
