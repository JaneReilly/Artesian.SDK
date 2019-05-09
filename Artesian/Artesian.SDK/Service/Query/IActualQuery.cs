// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using NodaTime;

namespace Artesian.SDK.Service
{
    interface IActualQuery<T>: IQuery<T>
    {
        T InGranularity(Granularity granularity);
        T WithTimeTransform(int tr);
        T WithTimeTransform(SystemTimeTransform tr);
        T WithFillNull();
        T WithFillCustom(double value);
        T WithFillLatestValue(Period period);
        T WithFillNone();
    }
}