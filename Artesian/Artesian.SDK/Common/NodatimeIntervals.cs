using EnsureThat;
using NodaTime;
using NodaTime.Calendars;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artesian.SDK.Common
{
    /// <summary>
    /// DatePeriod enums
    /// </summary>
    public enum DatePeriod : byte
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Day = 2,
        Week = 3,
        Month = 4,
        Bimestral = 5,
        Trimestral = 6,
        Calendar = 7
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// TimePeriod enums
    /// </summary>
    public enum TimePeriod : byte
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Hour = 2,
        TenMinutes = 3,
        Minute = 4,
        QuarterHour = 5,
        HalfHour = 6
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Nodatime Intervals Extensions
    /// </summary>
    public static class NodatimeIntervalsEx
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool IsStartOfInterval(this LocalDateTime localTime, DatePeriod period)
        {
            return localTime.AtStartOfInterval(period) == localTime;
        }

        public static bool IsStartOfInterval(this LocalDateTime localTime, TimePeriod period)
        {
            return localTime.AtStartOfInterval(period) == localTime;
        }

        public static LocalDateTime AtStartOfInterval(this LocalDateTime localTime, TimePeriod period)
        {
            return (localTime.InUtc() - _offsetFromStart(localTime.TimeOfDay, period)).LocalDateTime;
        }

        public static LocalDateTime AtStartOfInterval(this LocalDateTime localTime, DatePeriod period)
        {
            return localTime.Date.AtStartOfInterval(period).AtMidnight();
        }

        private static Duration _offsetFromStart(LocalTime time, TimePeriod period)
        {
            var offset = Duration.Zero;
            offset += Duration.FromTicks(time.TickOfSecond);
            offset += Duration.FromMilliseconds(time.Second);

            switch (period)
            {
                case TimePeriod.Hour:
                    offset += Duration.FromMinutes(time.Minute);
                    break;
                case TimePeriod.Minute:
                    break;
                case TimePeriod.TenMinutes:
                    offset += Duration.FromMinutes(time.Minute % 10);
                    break;
                case TimePeriod.QuarterHour:
                    offset += Duration.FromMinutes(time.Minute % 15);
                    break;
                case TimePeriod.HalfHour:
                    offset += Duration.FromMinutes(time.Minute % 30);
                    break;
            }

            return offset;
        }

        public static bool IsStartOfInterval(this LocalDate date, DatePeriod period)
        {
            return date.AtStartOfInterval(period) == date;
        }

        public static LocalDate AtStartOfInterval(this LocalDate date, DatePeriod period)
        {
            switch (period)
            {
                case DatePeriod.Day:
                    return date;
                case DatePeriod.Week:
                    return date.FirstDayOfTheWeek();
                case DatePeriod.Month:
                    return date.FirstDayOfTheMonth();
                case DatePeriod.Bimestral:
                    return new LocalDate(date.Year, ((date.Month - 1) / 2) * 2 + 1, 1, date.Calendar);
                case DatePeriod.Trimestral:
                    return date.FirstDayOfTheQuarter();
                case DatePeriod.Calendar:
                    return date.FirstDayOfTheYear();
            }

            return date;
        }
        public static LocalDate FirstDayOfTheWeek(this LocalDate date, IsoDayOfWeek dayOfWeek = IsoDayOfWeek.Monday)
        {
            Ensure.Bool.IsTrue(date.Calendar == CalendarSystem.Iso);
            return LocalDate.FromWeekYearWeekAndDay(WeekYearRules.Iso.GetWeekYear(date), WeekYearRules.Iso.GetWeekOfWeekYear(date), dayOfWeek);
        }

        public static LocalDate FirstDayOfTheMonth(this LocalDate date)
        {
            Ensure.Bool.IsTrue(date.Calendar == CalendarSystem.Iso);
            return new LocalDate(date.Year, date.Month, 1, date.Calendar);
        }

        public static LocalDate FirstDayOfTheQuarter(this LocalDate date)
        {
            Ensure.Bool.IsTrue(date.Calendar == CalendarSystem.Iso);
            return new LocalDate(date.Year, (int)((date.Month - 1) / 3) * 3 + 1, 1, date.Calendar);
        }

        public static LocalDate FirstDayOfTheSeason(this LocalDate date)
        {
            Ensure.Bool.IsTrue(date.Calendar == CalendarSystem.Iso);
            if (date.Month >= 10)
                return new LocalDate(date.Year, 10, 1, date.Calendar);
            else if (date.Month < 4)
                return new LocalDate(date.Year - 1, 10, 1, date.Calendar);
            else
                return new LocalDate(date.Year, 4, 1, date.Calendar);
        }

        public static LocalDate FirstDayOfTheYear(this LocalDate date)
        {
            Ensure.Bool.IsTrue(date.Calendar == CalendarSystem.Iso);
            return new LocalDate(date.Year, 1, 1, date.Calendar);
        }

        public static LocalDate LastDayOfTheWeek(this LocalDate date, IsoDayOfWeek dayOfWeek = IsoDayOfWeek.Sunday)
        {
            Ensure.Bool.IsTrue(date.Calendar == CalendarSystem.Iso);
            return LocalDate.FromWeekYearWeekAndDay(WeekYearRules.Iso.GetWeekYear(date), WeekYearRules.Iso.GetWeekOfWeekYear(date), dayOfWeek);
        }

        public static LocalDate LastDayOfTheMonth(this LocalDate date)
        {
            Ensure.Bool.IsTrue(date.Calendar == CalendarSystem.Iso);
            return date.FirstDayOfTheMonth().PlusMonths(1).Minus(Period.FromDays(1));
        }

        public static LocalDate LastDayOfTheQuarter(this LocalDate date)
        {
            return date.FirstDayOfTheMonth().PlusMonths(3).Minus(Period.FromDays(1));
        }

        public static LocalDate LastDayOfTheSeason(this LocalDate date)
        {
            Ensure.Bool.IsTrue(date.Calendar == CalendarSystem.Iso);
            if (date.Month >= 10)
                return new LocalDate(date.Year + 1, 3, 31, date.Calendar);
            else if (date.Month < 4)
                return new LocalDate(date.Year, 3, 31, date.Calendar);
            else
                return new LocalDate(date.Year, 9, 30, date.Calendar);
        }

        public static LocalDate LastDayOfTheYear(this LocalDate date)
        {
            Ensure.Bool.IsTrue(date.Calendar == CalendarSystem.Iso);
            return date.FirstDayOfTheYear().PlusYears(1).Minus(Period.FromDays(1));
        }

        public static LocalDate PreviousDayOfWeek(this LocalDate date, IsoDayOfWeek dayOfWeek = IsoDayOfWeek.Monday)
        {
            Ensure.Bool.IsTrue(date.Calendar == CalendarSystem.Iso);
            while (date.DayOfWeek != dayOfWeek)
                date = date.PlusDays(-1);

            return date;
        }

        public static LocalDate NextDayOfWeek(this LocalDate date, IsoDayOfWeek dayOfWeek = IsoDayOfWeek.Monday)
        {
            Ensure.Bool.IsTrue(date.Calendar == CalendarSystem.Iso);
            while (date.DayOfWeek != dayOfWeek)
                date = date.PlusDays(1);

            return date;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
