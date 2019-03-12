// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Actual Query Paramaters DTO
    /// </summary>
    public class ActualQueryParamaters : QueryParamaters
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Granularity? granularity;
        public int? tr;
        /// <summary>
        /// Actual Query Paramaters
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="granularity"></param>
        /// <param name="tr"></param>
        public ActualQueryParamaters(IEnumerable<int> ids,  Granularity? granularity, int? tr)
        {
            this.ids = ids;
            this.granularity = granularity;
            this.tr = tr;
        }
     
    }
}
