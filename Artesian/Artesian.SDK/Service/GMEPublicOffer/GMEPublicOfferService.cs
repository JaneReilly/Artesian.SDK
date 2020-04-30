using Flurl;

namespace Artesian.SDK.Service.GMEPublicOffer
{
    /// <summary>
    /// GMEPublicOfferService class
    /// Contains query types to be created
    /// </summary>
    public partial class GMEPublicOfferService : IGMEPublicOfferService
    {
        private IArtesianServiceConfig _cfg;
        private ArtesianPolicyConfig _policy;
        private Client _client;

        /// <summary>
        /// GME Public offer service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        public GMEPublicOfferService(IArtesianServiceConfig cfg)
            : this(cfg, new ArtesianPolicyConfig())
        {
        }

        /// <summary>
        /// GME Public offer service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        /// <param name="policy">ArtesianPolicyConfig</param>
        public GMEPublicOfferService(IArtesianServiceConfig cfg, ArtesianPolicyConfig policy)
        {
            _cfg = cfg;
            _policy = policy;
            _client = new Client(cfg, ArtesianConstants.GMEPublicOfferRoute.AppendPathSegment(ArtesianConstants.GMEPublicOfferVersion), _policy);
        }

    }
}
