using System;
using Artesian.SDK.Service;
using Flurl.Http.Testing;
using System.Net.Http;
using NodaTime;
using NUnit.Framework;
using Flurl;
using Artesian.SDK.Dto;

namespace Artesian.SDK.Tests
{
    /// <summary>
    /// Summary description for MarketAssessmentQueries
    /// </summary>
    [TestFixture]
    public class MarketAssessmentQueries
    {
        private ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region MarketData Ids
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
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
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

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromMonths(6))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P6M"
                    .SetQueryParam("id", new int[] { 100000001, 100000002, 100000003 })
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
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" })
                    .SetQueryParam("tz", "UTC"))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("WET")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" })
                    .SetQueryParam("tz", "WET"))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void MasWithHeaders()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void Mas_Partitioned_By_ID()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var act = qs.CreateMarketAssessment()
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
                    .ForProducts(new string[] { "M+1", "GY+1" })
                    .ExecuteAsync().ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D"
                    .SetQueryParam("id", new int[] {
                        100001250, 100001251, 100001252, 100001253 , 100001254,
                        100001255 , 100001256, 100001257, 100001258, 100001259,
                        100001260, 100001261, 100001262, 100001263, 100001264,
                        100001265, 100001266, 100001267, 100001268, 100001269,
                        100001270, 100001271, 100001272, 100001273, 100001274
                    })
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D"
                    .SetQueryParam("id", new int[] {
                        100001275, 100001276, 100001277, 100001278, 100001279,
                        100001280, 100001281, 100001282, 100001283, 100001284,
                        100001285, 100001286, 100001287, 100001289, 100001290,
                        100001291, 100001292, 100001293, 100001294, 100001295,
                        100001296, 100001297, 100001298, 100001299, 100001301
                    })
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D"
                    .SetQueryParam("id", new int[] {
                        100001302, 100001303, 100001304, 100001305, 100001306,
                        100001307, 100001308, 100001309, 100001310, 100001311,
                        100001312, 100001313, 100001314, 100001315, 100001315
                    })
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }
        #endregion

        #region FilterId
        [Test]
        public void MasInRelativeIntervalExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/RollingMonth"
                    .SetQueryParam("filterId", 1)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void MasInAbsoluteDateRangeExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("filterId", 1)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void MasInRelativePeriodExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InRelativePeriod(Period.FromDays(5))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/P5D"
                    .SetQueryParam("filterId", 1)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void MasInRelativePeriodRangeExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D"
                    .SetQueryParam("filterId", 1)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void MasWithTimeZone_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("filterId", 1)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" })
                    .SetQueryParam("tz", "UTC"))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("WET")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("filterId", 1)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" })
                    .SetQueryParam("tz", "WET"))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void MasWithHeaders_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("filterId", 1)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }
        #endregion

        #region Filler
        [Test]
        public void FillerNullMasInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillNull()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" })
                    .SetQueryParam("fillerK",FillerKindType.Null))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void FillerNoFillMasInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillNone()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" })
                    .SetQueryParam("fillerK", FillerKindType.NoFill))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }
        
        [Test]
        public void FillerLatestValidValueMasInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillLatestValue(Period.FromDays(7))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" })
                    .SetQueryParam("fillerK", FillerKindType.LatestValidValue)
                    .SetQueryParam("fillerP","P7D"))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void FillerCustomValueMasInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillCustomValue(new MarketAssessmentValue { Settlement = 123, Open = 456, Close = 789, High = 321, Low = 654, VolumePaid = 987, VolumeGiven = 213, Volume = 435 })
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" })
                    .SetQueryParam("fillerK", FillerKindType.CustomValue)
                    .SetQueryParam("fillerDVs",123)
                    .SetQueryParam("fillerDVo", 456)
                    .SetQueryParam("fillerDVc", 789)
                    .SetQueryParam("fillerDVh", 321)
                    .SetQueryParam("fillerDVl", 654)
                    .SetQueryParam("fillerDVvp", 987)
                    .SetQueryParam("fillerDVvg", 213)
                    .SetQueryParam("fillerDVvt", 435)
                    )
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void FillerCustomValueOHLCMasInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillCustomValue(new MarketAssessmentValue { Open = 456, Close = 789, High = 321, Low = 654 })
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" })
                    .SetQueryParam("fillerK", FillerKindType.CustomValue)
                    .SetQueryParam("fillerDVo", 456)
                    .SetQueryParam("fillerDVc", 789)
                    .SetQueryParam("fillerDVh", 321)
                    .SetQueryParam("fillerDVl", 654)
                    )
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }
        #endregion

        [Test]
        public void MasExtractionWindow_MktChange()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateMarketAssessment()
                           .ForMarketData(new int[] { 100000001 })
                           .ForProducts(new string[] { "M+1", "GY+1" })
                           .InRelativeInterval(RelativeInterval.RollingMonth);


                var test1 = partialQuery
                    .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/RollingMonth"
                    .SetQueryParam("id", 100000001)
                    .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);

                var test2 = partialQuery
                            .ForFilterId(1)
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/RollingMonth"
                        .SetQueryParam("filterId", 1)
                        .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                var test3 = partialQuery
                            .ForMarketData(new int[] { 100000004, 100000005, 100000006 })
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/RollingMonth"
                        .SetQueryParam("id", new int[] { 100000004, 100000005, 100000006 })
                        .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                        .WithVerb(HttpMethod.Get)
                        .Times(1);
            }
        }

        [Test]
        public void MasExtractionWindow_RelativeIntervalChange()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateMarketAssessment()
                           .ForMarketData(new int[] { 100000001 })
                           .ForProducts(new string[] { "M+1", "GY+1" })
                           .InRelativeInterval(RelativeInterval.RollingMonth);


                var test1 = partialQuery
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/RollingMonth"
                        .SetQueryParam("id", 100000001)
                        .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                var test2 = partialQuery
                            .InRelativePeriod(Period.FromDays(5))
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/P5D"
                        .SetQueryParam("id", 100000001)
                        .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                var test3 = partialQuery
                            .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D"
                        .SetQueryParam("id", 100000001)
                        .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                var test4 = partialQuery
                            .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                            .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10"
                        .SetQueryParam("id", 100000001)
                        .SetQueryParam("p", new string[] { "M+1", "GY+1" }))
                        .WithVerb(HttpMethod.Get)
                        .Times(1);


            }
        }
    }
}
