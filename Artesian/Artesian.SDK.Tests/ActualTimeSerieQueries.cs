using System;
using System.Net.Http;
using Artesian.SDK.Dto;
using Artesian.SDK.Service;
using Flurl;
using Flurl.Http.Testing;
using NodaTime;
using NUnit.Framework;

namespace Artesian.SDK.Tests
{
    [TestFixture]
    public class ActualTimeSerieQueries
    {
        private ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region MarketData ids
        [Test]
        public void ActInRelativeIntervalExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/RollingMonth")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .Times(1);
            }
        }

        [Test]
        public void ActInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .Times(1);
            }
        }

        [Test]
        public void ActInRelativePeriodExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/P5D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .Times(1);
            }
        }

        [Test]
        public void ActInRelativePeriodRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .Times(1);
            }
        }

        [Test]
        public void ActMultipleMarketDataWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] { 100000001, 100000002, 100000003 })
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Month)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromMonths(6))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Month/P2W/P6M")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] { 100000001, 100000002, 100000003 })
                    .Times(1);
            }
        }

        [Test]
        public void ActWithTimeZone()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .WithQueryParamValue("tz", "UTC")
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Hour)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("WET")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Hour/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .WithQueryParamValue("tz", "WET")
                    .Times(1);
            }
        }

        [Test]
        public void ActWithTimeTransfrom()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .WithQueryParamValue("tr", 1)
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithTimeTransform(SystemTimeTransform.THERMALYEAR)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .WithQueryParamValue("tr", 2)
                    .Times(1);
            }
        }

        [Test]
        public void ActWithHeaders()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D"
                    .SetQueryParam("id", 100000001))
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .WithHeader("X-Api-Key", TestConstants.APIKey)
                    .Times(1);
            }
        }

        [Test]
        public void Act_Partitioned_By_ID()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                    .ForMarketData(new int[] {
                        100001250, 100001251, 100001252, 100001253, 100001254,
                        100001255, 100001256, 100001257, 100001258, 100001259,
                        100001260, 100001261, 100001262, 100001263, 100001264,
                        100001265, 100001266, 100001267, 100001268, 100001269,
                        100001270, 100001271, 100001272, 100001273, 100001274,
                        100001275, 100001276, 100001277, 100001278, 100001279,
                        100001280, 100001281, 100001282, 100001283, 100001284,
                        100001285, 100001286, 100001287, 100001289, 100001290,
                        100001291, 100001292, 100001293, 100001294, 100001295,
                        100001296, 100001297, 100001298, 100001299, 100001301,
                        100001302, 100001303, 100001304, 100001305, 100001306,
                        100001307, 100001308, 100001309, 100001310, 100001311,
                        100001312, 100001313, 100001314, 100001315, 100001315 })
                    .InGranularity(Granularity.Day)
                    .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                    .ExecuteAsync().ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] {
                        100001250, 100001251, 100001252, 100001253 , 100001254,
                        100001255 , 100001256, 100001257, 100001258, 100001259,
                        100001260, 100001261, 100001262, 100001263, 100001264,
                        100001265, 100001266, 100001267, 100001268, 100001269,
                        100001270, 100001271, 100001272, 100001273, 100001274
                    })
                    .Times(1);

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] {
                        100001275, 100001276, 100001277, 100001278, 100001279,
                        100001280, 100001281, 100001282, 100001283, 100001284,
                        100001285, 100001286, 100001287, 100001289, 100001290,
                        100001291, 100001292, 100001293, 100001294, 100001295,
                        100001296, 100001297, 100001298, 100001299, 100001301
                    })
                    .Times(1);

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] {
                        100001302, 100001303, 100001304, 100001305, 100001306,
                        100001307, 100001308, 100001309, 100001310, 100001311,
                        100001312, 100001313, 100001314, 100001315, 100001315
                    })
                    .Times(1);
            }
        }
        #endregion

        #region FilterId
        [Test]
        public void ActInRelativeIntervalExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/RollingMonth")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("filterId", 1)
                    .Times(1);
            }
        }

        [Test]
        public void ActInAbsoluteDateRangeExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("filterId", 1)
                    .Times(1);
            }
        }

        [Test]
        public void ActInRelativePeriodExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/P5D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("filterId", 1)
                    .Times(1);
            }
        }

        [Test]
        public void ActInRelativePeriodRangeExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("filterId", 1)
                    .Times(1);
            }
        }

        [Test]
        public void ActWithTimeZone_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("filterId", 1)
                    .WithQueryParamValue("tz", "UTC")
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Hour)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("WET")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Hour/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("filterId", 1)
                    .WithQueryParamValue("tz", "WET")
                    .Times(1);
            }
        }

        [Test]
        public void ActWithTimeTransfrom_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("filterId", 1)
                    .WithQueryParamValue("tr", 1)
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithTimeTransform(SystemTimeTransform.THERMALYEAR)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("filterId", 1)
                    .WithQueryParamValue("tr", 2)
                    .Times(1);
            }
        }

        [Test]
        public void ActWithHeaders_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D"
                    .SetQueryParam("filterId", 1))
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .WithHeader("X-Api-Key", TestConstants.APIKey)
                    .Times(1);
            }
        }
        #endregion
    }
}
