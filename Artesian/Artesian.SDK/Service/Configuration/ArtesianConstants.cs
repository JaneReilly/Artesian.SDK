// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Artesian Constants
    /// </summary>
    public abstract class ArtesianConstants
    {
        internal const string CharacterValidatorRegEx = @"^[^'"",:;\s](?:(?:[^'"",:;\s]| )*[^'"",:;\s])?$";
        internal const string MarketDataNameValidatorRegEx = @"^[^\s](?:(?:[^\s]| )*[^\s])?$";
        internal const string QueryVersion = "v1.0";
        internal const string GMEPublicOfferVersion = "v1.0";
        internal const string QueryRoute = "query";
        internal const string GMEPublicOfferRoute = "gmepublicoffer";
        internal const string MetadataVersion = "v2.1";
        internal const int ServiceRequestTimeOutMinutes = 10;

    }
}
