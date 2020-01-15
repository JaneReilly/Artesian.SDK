using System;
using System.Collections.Generic;
using System.Text;

namespace Artesian.SDK.Service.PublicOffer
{
    /// <summary>
    /// Public Offer Service Interface
    /// </summary>
    public interface IPublicOfferService
    {
        /// <summary>
        /// Create Public Offer Query
        /// </summary>
        /// <returns></returns>
        PublicOfferQuery CreateRawCurveQuery();
    }
}
