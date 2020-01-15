using Flurl;

namespace Artesian.SDK.Service.PublicOffer
{
    /// <summary>
    /// PublicOfferService class
    /// Contains query types to be created
    /// </summary>
    public partial class PublicOfferService
    {
        /// <summary>
        /// Create Public Offer Query
        /// </summary>
        /// <returns>
        /// Public Offer Query <see cref="PublicOfferQuery"/>
        /// </returns>
        public PublicOfferQuery CreateRawCurveQuery()
        {
            return new PublicOfferQuery(_client);
        }
    }
}
