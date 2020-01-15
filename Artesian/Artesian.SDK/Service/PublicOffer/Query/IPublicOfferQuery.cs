using Artesian.SDK.Dto.PublicOffer;
using NodaTime;

namespace Artesian.SDK.Service.PublicOffer
{
    interface IPublicOfferQuery
    {
        PublicOfferQuery ForDate(LocalDate date);
        PublicOfferQuery ForPurpose(Purpose purpose);
        PublicOfferQuery ForStatus(Status status);
        PublicOfferQuery ForOperator(string[] @operator);
        PublicOfferQuery ForUnit(string[] unit);
        PublicOfferQuery ForMarket(Market[] market);
        PublicOfferQuery ForScope(Scope[] scope);
        PublicOfferQuery ForBAType(BAType[] baType);
        PublicOfferQuery ForZone(Zone[] zone);
        PublicOfferQuery ForUnitType(UnitType[] unitType);
        PublicOfferQuery ForGenerationType(GenerationType[] generationType);
        PublicOfferQuery WithSort(string[] sort);
        PublicOfferQuery WithPagination(int page, int pageSize);
    }
}
