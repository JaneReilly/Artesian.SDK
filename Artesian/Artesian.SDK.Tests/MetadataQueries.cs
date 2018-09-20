﻿using System;
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

        #region MarketData
        [Test]
        public void MarketData_ReadMarketDataByProviderCurveName()
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
        public void MarketData_ReadMarketDataByCurveRange()
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
        public void MarketData_ReadMarketDataRegistryAsync()
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
        public void MarketData_RegisterMarketDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var marketDataEntity = new MarketDataEntity.Input()
                {
                    ProviderName = "Test",
                    MarketDataName = "TestName",
                    OriginalGranularity = Granularity.Day,
                    OriginalTimezone = "CET",
                    AggregationRule = AggregationRule.Undefined,
                    Type = MarketDataType.VersionedTimeSerie
                };

                var mdq = mds.RegisterMarketDataAsync(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/marketdata/entity")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void MarketData_UpdateMarketDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var marketDataEntity = new MarketDataEntity.Input()
                {
                    ProviderName = "Test",
                    MarketDataName = "TestName",
                    OriginalGranularity = Granularity.Day,
                    OriginalTimezone = "CET",
                    AggregationRule = AggregationRule.Undefined,
                    Type = MarketDataType.VersionedTimeSerie,
                    MarketDataId = 1
                };

                var mdq = mds.UpdateMarketDataAsync(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/marketdata/entity/1")
                    .WithVerb(HttpMethod.Put)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void MarketData_DeleteMarketDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                mds.DeleteMarketDataAsync(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/marketdata/entity/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }
        #endregion

        #region SearchFacet
        [Test]
        public void SearchFacet_SearchFacetAsync()
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
                    .SetQueryParam("page", 1)
                    .SetQueryParam("searchText", "testText")
                    .SetQueryParam("filters", "TestKey%3ATestValue", true)
                    )
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }
        #endregion

        #region Operations
        [Test]
        public void Operations_SearchFacetAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var operations = new Operations()
                {
                    IDS = new HashSet<MarketDataETag>() { (new MarketDataETag(0, "provaEtag")) },
                    OperationList = new List<OperationParams>() { new  OperationParams()}
                };

                var mdq = mds.PerformOperationsAsync(operations).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }
        #endregion

        #region UpsertCurve
        [Test]
        public void UpsertCurve_UpsertCurveDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var data = new UpsertCurveData()
                {
                    Timezone = TimeZone.CurrentTimeZone.ToString(),
                };

                mds.UpsertCurveDataAsync(data).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/marketdata/upsertdata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }
        #endregion

        #region TimeTransform
        [Test]
        public void TimeTransform_ReadTimeTransformBaseAsync()
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
        public void TimeTransform_ReadTimeTransform()
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
                    .Times(1);
            }
        }

        [Test]
        public void TimeTransform_ReadTimeTransformWithHeaders()
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
        public void TimeTransform_RegisterTimeTransformBaseAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var timeTransformEntity = new TimeTransformSimpleShift()
                {
                    ID = 1,
                    Name = "TimeTName",
                    ETag = Guid.Empty,
                    DefinedBy = TransformDefinitionType.System,
                    Period = Granularity.Year,
                    PositiveShift = "",
                    NegativeShift = "P3M",
                };

                var mdq = mds.RegisterTimeTransformBaseAsync(timeTransformEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/timeTransform/entity")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void TimeTransform_UpdateTimeTransformBaseAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var timeTransformEntity = new TimeTransformSimpleShift()
                {
                    ID = 1,
                    Name = "TimeTName",
                    ETag = Guid.Empty,
                    DefinedBy = TransformDefinitionType.System,
                    Period = Granularity.Year,
                    PositiveShift = "",
                    NegativeShift = "P3M",
                };

                var mdq = mds.UpdateTimeTransformBaseAsync(timeTransformEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/timeTransform/entity/1")
                    .WithVerb(HttpMethod.Put)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void TimeTransform_DeleteTimeTransformSimpleShiftAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                mds.DeleteTimeTransformSimpleShiftAsync(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/timeTransform/entity/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }
        #endregion

        #region CustomFilter
        [Test]
        public void CustomFilter_CreateFilter()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var filter = new CustomFilter()
                {
                    Id = 1,
                    SearchText = "Text",
                    Name = "TestName"
                };

                var mdq = mds.CreateFilter(filter).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/filter")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void CustomFilter_UpdateFilter()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var filter = new CustomFilter()
                {
                    Id = 1,
                    SearchText = "Text",
                    Name = "TestName"
                };

                var mdq = mds.UpdateFilter(1, filter).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/filter/1")
                    .WithVerb(HttpMethod.Put)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void CustomFilter_RemoveFilter()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var filter = new CustomFilter();

                var mdq = mds.RemoveFilter(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/filter/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void CustomFilter_ReadFilter()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var filter = new CustomFilter();

                var mdq = mds.ReadFilter(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/filter/")
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void CustomFilter_ReadFilters()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var filter = new CustomFilter();

                var mdq = mds.ReadFilters(1, 1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/filter"
                    .SetQueryParam("pageSize", 1)
                    .SetQueryParam("page", 1))
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }
        #endregion

        #region Acl
        [Test]
        public void Acl_ReadRolesByPath()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var path = new PathString(new[] { "Path1" });

                var mdq = mds.ReadRolesByPath(path).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/acl/me")
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void Acl_GetRoles()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var mdq = mds.GetRoles(1, 1, new[] { "Principals" }).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/acl"
                    .SetQueryParam("pageSize", 1)
                    .SetQueryParam("page", 1)
                    .SetQueryParam("principalIds", "Principals")
                    .SetQueryParam("asOf", null))
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void Acl_AddRoles()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var auth = new AuthorizationPath.Input()
                {
                    Path = "",
                    Roles = new List<AuthorizationPrincipalRole>()
                    {
                        new AuthorizationPrincipalRole()
                        {
                            Role = "Role",
                            InheritedFrom = "InheritedFrom",
                            Principal = new Principal()
                            {
                                PrincipalId = "Id",
                                PrincipalType = PrincipalType.User
                            }
                        }
                    }
                };

                mds.AddRoles(auth).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/acl/roles")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void Acl_UpsertRoles()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var auth = new AuthorizationPath.Input()
                {
                    Path = "",
                    Roles = new List<AuthorizationPrincipalRole>()
                    {
                        new AuthorizationPrincipalRole()
                        {
                            Role = "Role",
                            InheritedFrom = "InheritedFrom",
                            Principal = new Principal()
                            {
                                PrincipalId = "Id",
                                PrincipalType = PrincipalType.User
                            }
                        }
                    }
                };

                mds.UpsertRoles(auth).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/acl")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void Acl_RemoveRoles()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var auth = new AuthorizationPath.Input()
                {
                    Path = "",
                    Roles = new List<AuthorizationPrincipalRole>()
                    {
                        new AuthorizationPrincipalRole()
                        {
                            Role = "Role",
                            InheritedFrom = "InheritedFrom",
                            Principal = new Principal()
                            {
                                PrincipalId = "Id",
                                PrincipalType = PrincipalType.User
                            }
                        }
                    }
                };

                mds.RemoveRoles(auth).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/acl/roles")
                    .WithVerb(HttpMethod.Delete)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }
        #endregion

        #region Admin
        [Test]
        public void Admin_CreateAuthGroup()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var group = new AuthGroup()
                {
                    ID = 1,
                    Name = "AuthGroupTest"
                };

                var mdq = mds.CreateAuthGroup(group).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/group")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void Admin_UpdateAuthGroup()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var group = new AuthGroup()
                {
                    ID = 1,
                    Name = "AuthGroupTest"
                };

                var mdq = mds.UpdateAuthGroup(1, group).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/group/1")
                    .WithVerb(HttpMethod.Put)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void Admin_RemoveAuthGroup()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                mds.RemoveAuthGroup(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/group/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void Admin_ReadAuthGroup()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                mds.ReadAuthGroup(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/group/1")
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void Admin_ReadAuthGroups()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var mdq = mds.ReadAuthGroups(1, 1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/group"
                    .SetQueryParam("pageSize", 1)
                    .SetQueryParam("page", 1))
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }
        #endregion

        #region ApiKey
        [Test]
        public void ApiKey_CreateApiKeyAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var apiKey = new ApiKey.Input()
                {
                    Id = 0
                };

                var mdq = mds.CreateApiKeyAsync(apiKey).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/apikey/entity")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void ApiKey_ReadApiKeyByIdAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var mdq = mds.ReadApiKeyByIdAsync(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/apikey/entity/1")
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void ApiKey_ReadApiKeyByKeyAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var mdq = mds.ReadApiKeyByKeyAsync("testKey").ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/apikey/entity"
                    .SetQueryParam("key", "testKey"))
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void ApiKey_ReadApiKeysAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                var mdq = mds.ReadApiKeysAsync(1, 1, "testName").ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/apikey/entity"
                    .SetQueryParam("pageSize", 1)
                    .SetQueryParam("page", 1)
                    .SetQueryParam("userId", "testName"))
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }

        [Test]
        public void ApiKey_DeleteApiKeyAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MetadataService(_cfg);

                mds.DeleteApiKeyAsync(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}v2.1/apikey/entity/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                    .Times(1);
            }
        }
        #endregion
    }
}
