using Flurl;

namespace Artesian.SDK.Service.PublicOffer
{
    /// <summary>
    /// PublicOfferService class
    /// Contains query types to be created
    /// </summary>
    public partial class PublicOfferService : IPublicOfferService
    {
        private IArtesianServiceConfig _cfg;
        private ArtesianPolicyConfig _policy;
        private Client _client;

        /// <summary>
        /// Public offer service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        public PublicOfferService(IArtesianServiceConfig cfg)
            : this(cfg, new ArtesianPolicyConfig())
        {
        }

        /// <summary>
        /// Public offer service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        /// <param name="policy">ArtesianPolicyConfig</param>
        public PublicOfferService(IArtesianServiceConfig cfg, ArtesianPolicyConfig policy)
        {
            _cfg = cfg;
            _policy = policy;
            _client = new Client(cfg, ArtesianConstants.PublicOfferRoute.AppendPathSegment(ArtesianConstants.PublicOfferVersion), _policy);
        }

    }
}
