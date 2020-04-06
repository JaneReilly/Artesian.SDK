// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;

namespace Artesian.SDK.Service
{
    interface IMasQuery<T>: IQueryWithFill<T>, IQueryWithExtractionInterval<T>, IQueryWithExtractionRange<T>
    {
        T ForProducts(params string[] products);
        T WithFillCustomValue(MarketAssessmentValue fillerDefaultValues);
    }
}