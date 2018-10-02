// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Granularity enums
    /// </summary>
    public enum Granularity
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
          Hour = 0
        , Day = 1
        , Week = 2
        , Month = 3
        , Quarter = 4
        , Year = 5
        , TenMinute = 6
        , FifteenMinute = 7
        , Minute = 8
        , ThirtyMinute = 9
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }


    /// <summary>
    /// GranularityExtensions
    /// </summary>
    public static partial class GranularityExtensions
    {

        /// <summary>
        /// Returns true if the given Granularity is  by time
        /// </summary>
        /// <param name="granularity">Granularity</param>
        public static bool IsTimeGranularity(this Granularity granularity)
        {
            return granularity.IsPartOf(Granularity.Day);
        }

        /// <summary>
        /// Returns true if the smaller is part of the biggest granularity
        /// </summary>
        /// <param name="smaller">Granularity</param>
        /// <param name="bigger">Granularity</param>
        public static bool IsPartOf(this Granularity smaller, Granularity bigger)
        {
            if (bigger == Granularity.Week)
                return smaller == Granularity.Day || smaller.IsPartOf(Granularity.Day);
            if (smaller == Granularity.Week)
                return false;

            if (bigger == Granularity.FifteenMinute)
                return smaller == Granularity.Minute;

            return smaller._orderOf() < bigger._orderOf();
        }

        /// <summary>
        /// Gives the number of minute based on Granularity
        /// </summary>
        /// <param name="granularity">Granularity</param>
        private static int _orderOf(this Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Minute: return 1;
                case Granularity.TenMinute: return 10;
                case Granularity.FifteenMinute: return 15;
                case Granularity.ThirtyMinute: return 30;
                case Granularity.Hour: return 60;
                case Granularity.Day: return 1440;
                case Granularity.Week: return 10080;
                case Granularity.Month: return 43200;
                case Granularity.Quarter: return 129600;
                case Granularity.Year: return 525600;
            }

            throw new InvalidOperationException($"Granularity {granularity} is not supported");
        }
    }

}
