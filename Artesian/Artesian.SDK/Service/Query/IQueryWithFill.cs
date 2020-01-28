// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime;

namespace Artesian.SDK.Service
{
    interface IQueryWithFill<T>: IBaseQuery<T>
    {
        T InRelativePeriod(Period extractionPeriod);
        T InRelativeInterval(RelativeInterval relativeInterval);
        T WithFillNull();
        T WithFillLatestValue(Period period);
        T WithFillNone();
    }
}