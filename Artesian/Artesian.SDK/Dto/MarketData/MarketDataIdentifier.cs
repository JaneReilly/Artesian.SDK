// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using System;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The MarketData identifier entity
    /// </summary>
    [MessagePackObject]
    public class MarketDataIdentifier : IEquatable<MarketDataIdentifier>
    {
        /// <summary>
        /// The MarketData identifier default constructor
        /// </summary>
        public MarketDataIdentifier()
        {
        }

        /// <summary>
        /// The MarketData identifier constructor by provider and name
        /// </summary>
        public MarketDataIdentifier(string provider, string name)
        {
            Provider = provider;
            Name = name;
        }

        /// <summary>
        /// The Provider unique name
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public virtual string Provider { get; set; }

        /// <summary>
        /// The Market Data unique name for the Provider
        /// </summary>
        [Required]
        [MessagePack.Key(1)]
        public virtual string Name { get; set; }

        /// <summary>
        /// The Market Data override for ToString()
        /// </summary>
        public override string ToString()
        {
            return string.Format("Provider: {0} Name: {1}", Provider, Name);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(MarketDataIdentifier other)
        {
            if (ReferenceEquals(this, other))
                return true;
            if (Equals(other, null))
                return false;

            return Provider == other.Provider
                && Name == other.Name;
        }

        public static bool operator ==(MarketDataIdentifier x, MarketDataIdentifier y)
        {
            if (!Equals(x, null))
                return x.Equals(y);
            else if (Equals(y, null))
                return true;
            else
                return false;

        }

        public static bool operator !=(MarketDataIdentifier x, MarketDataIdentifier y)
        {
            if (!Equals(x, null))
                return !x.Equals(y);
            else if (Equals(y, null))
                return false;
            else
                return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MarketDataIdentifier))
                return false;

            return Equals((MarketDataIdentifier)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 7243;
                hash = hash * 92821 + Provider.GetHashCode();
                hash = hash * 92821 + Name.GetHashCode();
                return hash;
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    internal static class MarketDataIdentifierExt
    {
        public static void Validate(this MarketDataIdentifier marketDataIdentifier)
        {
            ArtesianUtils.IsValidProvider(marketDataIdentifier.Provider,1,50);
            ArtesianUtils.IsValidMarketDataName(marketDataIdentifier.Name,1,250);
        }
    }

}
