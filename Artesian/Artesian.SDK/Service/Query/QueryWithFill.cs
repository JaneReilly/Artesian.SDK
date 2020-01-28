// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime;
using NodaTime.Text;
using System;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Query class
    /// </summary>
    public abstract class QueryWithFill<TQueryParams>: BaseQuery<TQueryParams> where TQueryParams : QueryWithFillParamaters, new()
    {        
        /// <summary>
        /// Query by relative period
        /// </summary>
        /// <param name="extractionPeriod">Period</param>
        /// <returns>Query</returns>
        protected QueryWithFill<TQueryParams> _inRelativePeriod(Period extractionPeriod)
        {
            _queryParamaters.ExtractionRangeType = ExtractionRangeType.Period;
            _queryParamaters.ExtractionRangeSelectionConfig.Period = extractionPeriod;
            return this;
        }

        /// <summary>
        /// Query by relative interval
        /// </summary>
        /// <param name="relativeInterval">RelativeInterval</param>
        /// <returns>Query</returns>
        protected QueryWithFill<TQueryParams> _inRelativeInterval(RelativeInterval relativeInterval)
        {
            _queryParamaters.ExtractionRangeType = ExtractionRangeType.RelativeInterval;
            _queryParamaters.ExtractionRangeSelectionConfig.Interval = relativeInterval;
            return this;
        }

        /// <summary>
        /// Build extraction range
        /// </summary>
        /// <returns>string</returns>
        protected string _buildExtractionRangeRoute(QueryWithFillParamaters queryParamaters)
        {
            string subPath;
            switch (queryParamaters.ExtractionRangeType)
            {
                case ExtractionRangeType.DateRange:
                    subPath = $"{_toUrlParam(queryParamaters.ExtractionRangeSelectionConfig.DateStart, queryParamaters.ExtractionRangeSelectionConfig.DateEnd)}";
                    break;
                case ExtractionRangeType.Period:
                    subPath = $"{queryParamaters.ExtractionRangeSelectionConfig.Period}";
                    break;
                case ExtractionRangeType.PeriodRange:
                    subPath = $"{queryParamaters.ExtractionRangeSelectionConfig.PeriodFrom}/{queryParamaters.ExtractionRangeSelectionConfig.PeriodTo}";
                    break;
                case ExtractionRangeType.RelativeInterval:
                    subPath = $"{queryParamaters.ExtractionRangeSelectionConfig.Interval}";
                    break;
                default:
                    throw new NotSupportedException("ExtractionRangeType");
            }

            return subPath;
        }

        /// <summary>
        /// Validate query
        /// </summary>
        /// <returns></returns>
        protected override void _validateQuery()
        {
            base._validateQuery();
        }

    }
}