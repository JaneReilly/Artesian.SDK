// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;

namespace Artesian.SDK.Service
{
    interface IActualQuery<T>: IQuery<T>, IQueryPartition
    {
        T InGranularity(Granularity granularity);
        T WithTimeTransform(int tr);
        T WithTimeTransform(SystemTimeTransform tr);
    }
}