// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime;

namespace Artesian.SDK.Service
{
    interface IMasQuery<T>: IQuery<T>
    {
        T ForProducts(params string[] products);
        T WithFillNull();
        T WithFillCustom(double settlement, double open, double close, double high, double low, double volumePaid, double volumeGiven, double volumeTotal);
        T WithFillLatestValue(Period period);
        T WithFillNone();
    }
}