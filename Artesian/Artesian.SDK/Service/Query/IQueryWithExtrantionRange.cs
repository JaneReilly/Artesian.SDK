// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime;

namespace Artesian.SDK.Service
{
    interface IQueryWithExtractionRange<T>: IQuery<T>
    {
        T InAbsoluteDateRange(LocalDate from, LocalDate to);
        T InRelativePeriodRange(Period from, Period to);
    }
}