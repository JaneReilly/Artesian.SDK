// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Query class
    /// </summary>
    public abstract class Query<TQueryParams> where TQueryParams : QueryParamaters, new()
    {
        /// <summary>
        /// Store for QueryParams
        /// </summary>
        protected TQueryParams _queryParamaters = new TQueryParams();

        private static LocalDatePattern _localDatePattern = LocalDatePattern.Iso;
        private static LocalDateTimePattern _localDateTimePattern = LocalDateTimePattern.ExtendedIso;
        
        /// <summary>
        /// Set the marketData id to be queried
        /// </summary>
        /// <param name="ids">Int[]</param>
        /// <returns>Query</returns>
        protected Query<TQueryParams> _forMarketData(int[] ids)
        {
            _queryParamaters.FilterId = null;

            _queryParamaters.Ids = ids;
            return this;
        }
        /// <summary>
        /// Set the filter id to be queried
        /// </summary>
        /// <param name="filterId">The filter id to be queried</param>
        /// <returns>Query</returns>
        protected Query<TQueryParams> _forFilterId(int filterId)
        {
            _queryParamaters.Ids = null;

            _queryParamaters.FilterId = filterId;
            return this;
        }
        /// <summary>
        /// Set the timezone to be queried
        /// </summary>
        /// <param name="tz">String timezone in IANA format</param>
        /// <returns>Query</returns>
        protected Query<TQueryParams> _inTimezone(string tz)
        {
            if (DateTimeZoneProviders.Tzdb.GetZoneOrNull(tz) == null)
                throw new ArgumentException($"Timezone {tz} is not recognized");
            _queryParamaters.TimeZone = tz;
            return this;
        }

        /// <summary>
        /// Query by absolute range
        /// </summary>
        /// <param name="start">Local date Start</param>
        /// <param name="end">Local date End</param>
        /// <returns>Query</returns>
        protected Query<TQueryParams> _inAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            if (end <= start)
                throw new ArgumentException("End date " + end + " must be greater than start date " + start);

            _queryParamaters.ExtractionRangeType = ExtractionRangeType.DateRange;
            _queryParamaters.ExtractionRangeSelectionConfig.DateStart = start;
            _queryParamaters.ExtractionRangeSelectionConfig.DateEnd = end;
            return this;
        }

        /// <summary>
        /// Query by relative period range
        /// </summary>
        /// <param name="from">Period Start</param>
        /// <param name="to">Period End</param>
        /// <returns>Query</returns>
        protected Query<TQueryParams> _inRelativePeriodRange(Period from, Period to)
        {
            _queryParamaters.ExtractionRangeType = ExtractionRangeType.PeriodRange;
            _queryParamaters.ExtractionRangeSelectionConfig.PeriodFrom = from;
            _queryParamaters.ExtractionRangeSelectionConfig.PeriodTo = to;
            return this;
        }

        /// <summary>
        /// Query by relative period
        /// </summary>
        /// <param name="extractionPeriod">Period</param>
        /// <returns>Query</returns>
        protected Query<TQueryParams> _inRelativePeriod(Period extractionPeriod)
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
        protected Query<TQueryParams> _inRelativeInterval(RelativeInterval relativeInterval)
        {
            _queryParamaters.ExtractionRangeType = ExtractionRangeType.RelativeInterval;
            _queryParamaters.ExtractionRangeSelectionConfig.Interval = relativeInterval;
            return this;
        }

        /// <summary>
        /// Build extraction range
        /// </summary>
        /// <returns>string</returns>
        protected string _buildExtractionRangeRoute(QueryParamaters queryParamaters)
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
        protected virtual void _validateQuery()
        {
            if (_queryParamaters.ExtractionRangeType == null)
                throw new ApplicationException("Data extraction range must be provided. Provide a date range , period or period range or an interval eg .InAbsoluteDateRange()");

            if (_queryParamaters.Ids == null && _queryParamaters.FilterId == null)
                throw new ApplicationException("Marketadata ids OR filterId must be provided for extraction. Use .ForMarketData() OR .ForFilterId() and provide an integer or integer array as an argument");

            if (_queryParamaters.Ids != null && _queryParamaters.FilterId != null)
                throw new ApplicationException("Marketadata ids AND filterId cannot be valorized at same time, choose one");
        }

        internal string _toUrlParam(LocalDate start, LocalDate end)
        {
            return $"{_localDatePattern.Format(start)}/{_localDatePattern.Format(end)}";
        }

        internal string _toUrlParam(LocalDateTime dateTime)
        {
            return _localDateTimePattern.Format(dateTime);
        }

    }
}