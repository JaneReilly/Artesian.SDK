// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using NodaTime;

namespace Artesian.SDK.Service
{
    interface IAuctionQuery<T>: IQuery<T>, IQueryWithExtractionRange<T>
    {
    }
}