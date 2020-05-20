using System;
using System.Collections.Generic;
using System.Net.Http;
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.GMEPublicOffer;
using Artesian.SDK.Service;
using Artesian.SDK.Service.GMEPublicOffer;
using Flurl;
using Flurl.Http.Testing;
using NodaTime;
using NUnit.Framework;

namespace Artesian.SDK.Tests
{
    [TestFixture]
    public class GMEPublicOfferQueries
    {
        private ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region Raw PO curve query
        [Test]
        public void ExtractRawCurveBasic()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = qs.CreateRawCurveQuery()
                       .ForDate(new LocalDate(2019,1,1))
                       .ForPurpose(Purpose.OFF)
                       .ForStatus(Status.INC)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}gmepublicoffer/v1.0/extract/2019-01-01/OFF/INC")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ExtractRawCurve()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = qs.CreateRawCurveQuery()
                       .ForDate(new LocalDate(2019, 1, 1))
                       .ForPurpose(Purpose.OFF)
                       .ForStatus(Status.INC)
                       .ForGenerationType(new GenerationType[] { GenerationType.GAS })
                       .ForBAType(new BAType[] { BAType.REV })
                       .ForMarket(new Market[] { Market.MB4 })
                       .ForOperator(new string[] { "op1" })
                       .ForScope(new Scope[] { Scope.GR1 })
                       .ForUnit(new string[] { "unit1" })
                       .ForUnitType(new UnitType[] { UnitType.UP })
                       .ForZone(new Zone[] { Zone.CNOR })
                       .WithPagination(2,20)
                       .WithSort(new string[] { "id asc" })
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}gmepublicoffer/v1.0/extract/2019-01-01/OFF/INC")
                    .WithQueryParamValue("generationType", "GAS")
                    .WithQueryParamValue("baType", "REV")
                    .WithQueryParamValue("market", "MB4")
                    .WithQueryParamValue("operators", "op1")
                    .WithQueryParamValue("scope", "GR1")
                    .WithQueryParamValue("unit", "unit1")
                    .WithQueryParamValue("unitType", "UP")
                    .WithQueryParamValue("zone", "CNOR")
                    .WithQueryParamValue("page", "2")
                    .WithQueryParamValue("pageSize", "20")
                    .WithQueryParamValue("sort", "id asc")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        #endregion


        #region Metadata 
        [Test]
        public void ReadOperatorsEnumVar1()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = qs.ReadOperatorsAsync(2,20)
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}gmepublicoffer/v1.0/enums/operators")
                    .WithQueryParamValue("page", 2)
                    .WithQueryParamValue("pageSize", 20)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadOperatorsEnumVar2()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = qs.ReadOperatorsAsync(2, 20, operatorFilter: "myFilter", sort: new string[] { "operator asc" })
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}gmepublicoffer/v1.0/enums/operators")
                    .WithQueryParamValue("page", 2)
                    .WithQueryParamValue("pageSize", 20)
                    .WithQueryParamValue("operatorFilter", "myFilter")
                    .WithQueryParamValue("sort", "operator asc")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadUnitsEnumVar1()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = qs.ReadUnitsAsync(2, 4)
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}gmepublicoffer/v1.0/enums/units")
                    .WithQueryParamValue("page", 2)
                    .WithQueryParamValue("pageSize", 4)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadUnitsEnumVar2()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = qs.ReadUnitsAsync(2, 20, unitFilter: "myFilter", sort: new string[] { "unit asc" })
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}gmepublicoffer/v1.0/enums/units")
                    .WithQueryParamValue("page", 2)
                    .WithQueryParamValue("pageSize", 20)
                    .WithQueryParamValue("unitFilter", "myFilter")
                    .WithQueryParamValue("sort", "unit asc")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }


        [Test]
        public void ReadUnitConfigurationMappingVar1()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var req = qs.ReadUnitConfigurationMappingAsync("myUnit")
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}gmepublicoffer/v1.0/unitconfigurationmappings/myUnit")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadUnitConfigurationMappingsVar1()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new GMEPublicOfferService(_cfg);

                var req = qs.ReadUnitConfigurationMappingsAsync(1, 20)
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}gmepublicoffer/v1.0/unitconfigurationmappings")
                    .WithQueryParamValue("page", 1)
                    .WithQueryParamValue("pageSize", 20)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadUnitConfigurationMappingsVar2()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new GMEPublicOfferService(_cfg);

                var req = qs.ReadUnitConfigurationMappingsAsync(1, 20, unitFilter: "unitFilterTest", sort: new string[] { "unit desc" })
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}gmepublicoffer/v1.0/unitconfigurationmappings")
                    .WithQueryParamValue("page", 1)
                    .WithQueryParamValue("pageSize", 20)
                    .WithQueryParamValue("unitFilter", "unitFilterTest")
                    .WithQueryParamValue("sort", "unit desc")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void UpsertUnitConfigurationMapping()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new GMEPublicOfferService(_cfg);

                var unitCfg = new UnitConfigurationDto() {
                
                    Unit = "unitName",
                    Mappings = new List<GenerationTypeMapping>() {
                        new GenerationTypeMapping(){
                            From = new LocalDate(2019,1,1),
                            To = new LocalDate(2020,1,1),
                            GenerationType = GenerationType.COAL
                        }
                    }
                };

                var req = qs.UpsertUnitConfigurationMappingAsync(unitCfg)
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}gmepublicoffer/v1.0/unitconfigurationmappings/{unitCfg.Unit}")
                    .WithVerb(HttpMethod.Put)
                    .Times(1);
            }
        }

        [Test]
        public void DeleteUnitConfigurationMapping()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new GMEPublicOfferService(_cfg);

                qs.DeleteUnitConfigurationMappingAsync("unitToDelete")
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}gmepublicoffer/v1.0/unitconfigurationmappings/unitToDelete")
                    .WithVerb(HttpMethod.Delete)
                    .Times(1);
            }
        }

        #endregion

    }
}
