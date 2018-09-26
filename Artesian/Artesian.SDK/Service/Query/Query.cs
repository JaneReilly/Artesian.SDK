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
    public abstract class Query
    {
        // must comment and document all methods
        private ExtractionRangeSelectionConfig _extractionRangeCfg = new ExtractionRangeSelectionConfig();
        private ExtractionRangeType? _extractionRangeType = null;
        private static LocalDatePattern _localDatePattern = LocalDatePattern.Iso;
        private static LocalDateTimePattern _localDateTimePattern = LocalDateTimePattern.ExtendedIso;

        /// <summary>
        /// MarketData identifiers
        /// </summary>
        protected IEnumerable<int> _ids;
        /// <summary>
        /// timezone
        /// </summary>
        protected string _tz;
        /// <summary>
        /// filterId
        /// </summary>
        protected int? _filterId;

        /// <summary>
        /// Set the marketData id to be queried
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Query</returns>
        protected Query _forMarketData(int[] ids)
        {
            _ids = ids;
            return this;
        }
        /// <summary>
        /// Set the filter id to be queried
        /// </summary>
        /// <param name="filterId">The filter id to be queried</param>
        /// <returns>Query</returns>
        protected Query _forFilterId(int filterId)
        {
            _filterId = filterId;
            return this;
        }
        /// <summary>
        /// Set the timezone to be queried
        /// </summary>
        /// <param name="tz"></param>
        /// <returns>Query</returns>
        protected Query _inTimezone(string tz)
        {
            if (DateTimeZoneProviders.Tzdb.GetZoneOrNull(tz) == null)
                throw new ArgumentException($"Timezone {tz} is not recognized");
            _tz = tz;
            return this;
        }

        /// <summary>
        /// Query by absolute range
        /// </summary>
        /// <param name="start">Local date Start</param>
        /// <param name="end">Local date End</param>
        /// <returns>Query</returns>
        protected Query _inAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            if (end <= start)
                throw new ArgumentException("End date " + end + " must be greater than start date " + start);

            _extractionRangeType = ExtractionRangeType.DateRange;
            _extractionRangeCfg.DateStart = start;
            _extractionRangeCfg.DateEnd = end;
            return this;
        }

        /// <summary>
        /// Query by relative period range
        /// </summary>
        /// <param name="from">Period Start</param>
        /// <param name="to">Period End</param>
        /// <returns>Query</returns>
        protected Query _inRelativePeriodRange(Period from, Period to)
        {
            _extractionRangeType = ExtractionRangeType.PeriodRange;
            _extractionRangeCfg.PeriodFrom = from;
            _extractionRangeCfg.PeriodTo = to;
            return this;
        }

        /// <summary>
        /// Query by relative period
        /// </summary>
        /// <param name="extractionPeriod">Period</param>
        /// <returns>Query</returns>
        protected Query _inRelativePeriod(Period extractionPeriod)
        {
            _extractionRangeType = ExtractionRangeType.Period;
            _extractionRangeCfg.Period = extractionPeriod;
            return this;
        }

        /// <summary>
        /// Query by relative interval
        /// </summary>
        /// <param name="relativeInterval">RelativeInterval</param>
        /// <returns>Query</returns>
        protected Query _inRelativeInterval(RelativeInterval relativeInterval)
        {
            _extractionRangeType = ExtractionRangeType.RelativeInterval;
            _extractionRangeCfg.Interval = relativeInterval;
            return this;
        }

        /// <summary>
        /// Build extraction range
        /// </summary>
        /// <returns>string</returns>
        protected string _buildExtractionRangeRoute()
        {
            string subPath;
            switch (_extractionRangeType)
            {
                case ExtractionRangeType.DateRange:
                    subPath = $"{_toUrlParam(_extractionRangeCfg.DateStart,_extractionRangeCfg.DateEnd)}";
                    break;
                case ExtractionRangeType.Period:
                    subPath = $"{_extractionRangeCfg.Period}";
                    break;
                case ExtractionRangeType.PeriodRange:
                    subPath = $"{_extractionRangeCfg.PeriodFrom}/{_extractionRangeCfg.PeriodTo}";
                    break;
                case ExtractionRangeType.RelativeInterval:
                    subPath = $"{_extractionRangeCfg.Interval}";
                    break;
                default:
                    throw new Exception();
            }

            return subPath;
        }

        /// <summary>
        /// Validate query
        /// </summary>
        /// <returns></returns>
        protected virtual void _validateQuery()
        {
            if (_extractionRangeType == null)
                throw new ApplicationException("Data extraction range must be provided. Provide a date range , period or period range or a interval eg .InAbsoluteDateRange()");

            if (_ids == null && _filterId == null)
                throw new ApplicationException("Marketadata ids OR filterId must be provided for extraction. Use .ForMarketData() OR .ForFilterId() and provide a integer or integer array as an argument");

            if (_ids != null && _filterId != null)
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