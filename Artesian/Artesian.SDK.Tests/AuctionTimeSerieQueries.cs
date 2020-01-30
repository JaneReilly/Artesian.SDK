using Artesian.SDK.Service;
using Flurl;
using Flurl.Http.Testing;
using NodaTime;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace Artesian.SDK.Tests
{
    [TestFixture]
    public class AuctionTimeSerieQueries
    {
        private ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region MarketData ids

        [Test]
        public void AuctInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForMarketData(new int[] { 100000001 })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .Times(1);
            }
        }

        [Test]
        public void AuctInRelativePeriodRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForMarketData(new int[] { 100000001 })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .Times(1);
            }
        }

        [Test]
        public void AuctInRelativePeriodExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForMarketData(new int[] { 100000001 })
                       .InRelativePeriod(Period.FromWeeks(2))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/P2W")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .Times(1);
            }
        }

        [Test]
        public void AuctMultipleMarketDataWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] { 100000001, 100000002, 100000003 })
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromMonths(6))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/P2W/P6M")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] { 100000001, 100000002, 100000003 })
                    .Times(1);
            }
        }

        [Test]
        public void AuctWithTimeZone()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForMarketData(new int[] { 100000001 })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .WithQueryParamValue("tz", "UTC")
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForMarketData(new int[] { 100000001 })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("WET")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", 100000001)
                    .WithQueryParamValue("tz", "WET")
                    .Times(1);
            }
        }

        [Test]
        public void AuctWithHeaders()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForMarketData(new int[] { 100000001 })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/P2W/P20D"
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
        public void Auct_Partitioned_By_ID()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
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
                    .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                    .ExecuteAsync().ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] {
                        100001250, 100001251, 100001252, 100001253 , 100001254,
                        100001255 , 100001256, 100001257, 100001258, 100001259,
                        100001260, 100001261, 100001262, 100001263, 100001264,
                        100001265, 100001266, 100001267, 100001268, 100001269,
                        100001270, 100001271, 100001272, 100001273, 100001274
                    })
                    .Times(1);

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] {
                        100001275, 100001276, 100001277, 100001278, 100001279,
                        100001280, 100001281, 100001282, 100001283, 100001284,
                        100001285, 100001286, 100001287, 100001289, 100001290,
                        100001291, 100001292, 100001293, 100001294, 100001295,
                        100001296, 100001297, 100001298, 100001299, 100001301
                    })
                    .Times(1);

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] {
                        100001302, 100001303, 100001304, 100001305, 100001306,
                        100001307, 100001308, 100001309, 100001310, 100001311,
                        100001312, 100001313, 100001314, 100001315, 100001315
                    })
                    .Times(1);
            }
        }

        #endregion MarketData ids

        #region FilterId

        [Test]
        public void AuctInAbsoluteDateRangeExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForFilterId(1)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("filterId", 1)
                    .Times(1);
            }
        }

        [Test]
        public void AuctInRelativePeriodRangeExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForFilterId(1)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("filterId", 1)
                    .Times(1);
            }
        }

        [Test]
        public void AuctWithTimeZone_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForFilterId(1)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("filterId", 1)
                    .WithQueryParamValue("tz", "UTC")
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForFilterId(1)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("WET")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("filterId", 1)
                    .WithQueryParamValue("tz", "WET")
                    .Times(1);
            }
        }

        [Test]
        public void AuctWithHeaders_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateAuction()
                       .ForFilterId(1)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/auction/P2W/P20D"
                    .SetQueryParam("filterId", 1))
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .WithHeader("X-Api-Key", TestConstants.APIKey)
                    .Times(1);
            }
        }

        #endregion FilterId
    }
}