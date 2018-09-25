// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    public abstract class Query
    {
        // must comment and document all methods
        private ExtractionRangeSelectionConfig _extractionRangeCfg = new ExtractionRangeSelectionConfig();
        private ExtractionRangeType? _extractionRangeType = null;
        private static LocalDatePattern _localDatePattern = LocalDatePattern.Iso;
        private static LocalDateTimePattern _localDateTimePattern = LocalDateTimePattern.ExtendedIso;

        protected IEnumerable<int> _ids;
        protected string _tz;

        protected Query _forMarketData(int[] ids)
        {
            _ids = ids;
            return this;
        }

        protected Query _inTimezone(string tz)
        {
            if (DateTimeZoneProviders.Tzdb.GetZoneOrNull(tz) == null)
                throw new ArgumentException($"Timezone {tz} is not recognized");
            _tz = tz;
            return this;
        }

        protected Query _inAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            if (end <= start)
                throw new ArgumentException("End date " + end + " must be greater than start date " + start);

            _extractionRangeType = ExtractionRangeType.DateRange;
            _extractionRangeCfg.DateStart = start;
            _extractionRangeCfg.DateEnd = end;
            return this;
        }

        protected Query _inRelativePeriodRange(Period from, Period to)
        {
            _extractionRangeType = ExtractionRangeType.PeriodRange;
            _extractionRangeCfg.PeriodFrom = from;
            _extractionRangeCfg.PeriodTo = to;
            return this;
        }

        protected Query _inRelativePeriod(Period extractionPeriod)
        {
            _extractionRangeType = ExtractionRangeType.Period;
            _extractionRangeCfg.Period = extractionPeriod;
            return this;
        }

        protected Query _inRelativeInterval(RelativeInterval relativeInterval)
        {
            _extractionRangeType = ExtractionRangeType.RelativeInterval;
            _extractionRangeCfg.Interval = relativeInterval;
            return this;
        }

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

        protected virtual void _validateQuery()
        {
            if (_extractionRangeType == null)
                throw new ApplicationException("Data extraction range must be provided. Provide a date range , period or period range or a interval eg .InAbsoluteDateRange()");

            if (_ids == null)
                throw new ApplicationException("Marketadata ids must be provided for extraction. Use .ForMarketData() and provide a integer or integer array as an argument");
        }

        internal string _toUrlParam(LocalDate start, LocalDate end)
        {
            return $"{_localDatePattern.Format(start)}/{_localDatePattern.Format(end)}";
        }

        protected string _toUrlParam(LocalDateTime dateTime)
        {
            return _localDateTimePattern.Format(dateTime);
        }

    }
}