// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Query Paramaters DTO
    /// </summary>
    public  abstract class QueryParamaters
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IEnumerable<int> ids;
    }
}
