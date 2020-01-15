using System;
using System.Net.Http;
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.PublicOffer;
using Artesian.SDK.Service;
using Artesian.SDK.Service.PublicOffer;
using Flurl;
using Flurl.Http.Testing;
using NodaTime;
using NUnit.Framework;

namespace Artesian.SDK.Tests
{
    [TestFixture]
    public class PublicOfferQueries
    {
        private ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region Raw PO curve query
        [Test]
        public void ExtractRawCurveBasic()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new PublicOfferService(_cfg);

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

                var qs = new PublicOfferService(_cfg);

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

                var qs = new PublicOfferService(_cfg);

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

                var qs = new PublicOfferService(_cfg);

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

                var qs = new PublicOfferService(_cfg);

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

                var qs = new PublicOfferService(_cfg);

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

        #endregion

    }
}
