using System;
using System.Collections.Generic;
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
    public class MetadataQueries
    {
        private ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        [Test]
        public void ReadTimeTransformBaseWithTimeTransformID()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var mdq = mds.ReadTimeTransformBaseAsync(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/timeTransform/entity/1")
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var mdq = mds.ReadTimeTransformBaseAsync(2).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/timeTransform/entity/2")
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void ReadTimeTransform()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var mdq = mds.ReadTimeTransformsAsync(1, 1, true).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/timeTransform/entity"
                    .SetQueryParam("pageSize", 1)
                    .SetQueryParam("page",1)
                    .SetQueryParam("userDefined", true))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadMarketDataByProviderCurveName()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var mdq = mds.ReadMarketDataRegistryAsync(new MarketDataIdentifier("TestProvider", "TestCurveName")).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/marketdata/entity"
                    .SetQueryParam("provider", "TestProvider")
                    .SetQueryParam("curveName", "TestCurveName"))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadMarketDataByCurveRange()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var mdq = mds.ReadCurveRangeAsync(100000001,1,1,"M+1", new LocalDateTime(2018, 07, 19, 12, 0), new LocalDateTime(2017, 07, 19, 12, 0)).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/marketdata/entity/100000001/curves"
                    .SetQueryParam("versionFrom", new LocalDateTime(2018, 07, 19, 12, 0))
                    .SetQueryParam("versionTo", new LocalDateTime(2017, 07, 19, 12, 0))
                    .SetQueryParam("product","M+1")
                    .SetQueryParam("page", 1)
                    .SetQueryParam("pageSize", 1))
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadMarketDataRegistryAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var mdq = mds.ReadMarketDataRegistryAsync(100000001).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/marketdata/entity/100000001")
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void SearchFacetAsync()
        {
            using (var httpTest = new HttpTest())
            {
                Dictionary<string, string[]> filterDict = new Dictionary<string, string[]>
                {
                    {"TestKey",new string[]{"TestValue"} }
                };
                var mds = new MetadataService(_cfg);
                var filter = new ArtesianSearchFilter
                {
                    Page = 1,
                    PageSize = 1,
                    SearchText = "testText",
                    Filters = filterDict
                };
                var mdq = mds.SearchFacetAsync(filter).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/marketdata/searchfacet"
                    .SetQueryParam("pageSize", 1)
                    .SetQueryParam("page",1)
                    .SetQueryParam("searchText", "testText")
                    .SetQueryParam("filters", "TestKey%3ATestValue", true)
                    )
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadTimeTransformWithHeaders()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var mdq = mds.ReadTimeTransformsAsync(1, 1, true).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/timeTransform/entity"
                    .SetQueryParam("pageSize", 1)
                    .SetQueryParam("page", 1)
                    .SetQueryParam("userDefined", true))
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void RegisterMarketDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var marketDataEntity = new MarketDataEntity.Input();

                var mdq = mds.RegisterMarketDataAsync(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/marketdata/entity")
                    .WithVerb(HttpMethod.Post)
                    //.WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void UpdateMarketDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var marketDataEntity = new MarketDataEntity.Input()
                {
                    MarketDataId = 1
                };

                var mdq = mds.UpdateMarketDataAsync(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/marketdata/entity/1")
                    .WithVerb(HttpMethod.Put)
                    //.WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void DeleteMarketDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                mds.DeleteMarketDataAsync(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/marketdata/entity/1")
                    .WithVerb(HttpMethod.Delete)
                    //.WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }
    }
}
