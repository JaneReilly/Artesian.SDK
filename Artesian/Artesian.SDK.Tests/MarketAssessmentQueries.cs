﻿using System;
using Artesian.SDK.Service;
using Flurl.Http.Testing;
using System.Net.Http;
using NodaTime;
using NUnit.Framework;
using Flurl;

namespace Artesian.SDK.Tests
{
    /// <summary>
    /// Summary description for MarketAssessmentQueries
    /// </summary>
    [TestFixture]
    public class MarketAssessmentQueries
    {
        private ArtesianServiceConfig _cfg = new ArtesianServiceConfig();

        [Test]
        public void MasInRelativeIntervalExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/RollingMonth"
                    .SetQueryParam("id", 100000001))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);

            }
        }

        [Test]
        public void MasInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1","GY+1"})
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync().Result;


                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void MasInRelativePeriodExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InRelativePeriod(Period.FromDays(5))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/P5D"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);

            }
        }

        [Test]
        public void MasInRelativePeriodRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void MasMultipleMarketDataWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D"
                    .SetQueryParam("id" , new int[] { 100000001, 100000002, 100000003 })
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);

            }
        }

        [Test]
        public void MasWithTimeZone()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("CET")
                       .ExecuteAsync().Result;


                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" })
                    .SetQueryParam("tz", "CET"))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);

            }
        }
    }
}
