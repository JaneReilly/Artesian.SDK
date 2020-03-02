// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Service;
using MessagePack;
using Newtonsoft.Json;
using System;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The AuctionRow entity
    /// </summary>
    [MessagePackObject]
    public class AuctionRow
    {
        /// <summary>
        /// Provider Name
        /// </summary>
        [JsonProperty(PropertyName = "P")]
        [Key(0)]
        public virtual string ProviderName { get; set; }

        /// <summary>
        /// Curve Name
        /// </summary>
        [JsonProperty(PropertyName = "N")]
        [Key(1)]
        public virtual string CurveName { get; set; }

        /// <summary>
        /// Market Data ID
        /// </summary>
        [JsonProperty(PropertyName = "ID")]
        [Key(2)]
        public virtual int TSID { get; set; }

        /// <summary>
        /// Bid Timestamp
        /// </summary>
        [JsonProperty(PropertyName = "T")]
        [Key(3)]
        public virtual DateTimeOffset BidTimestamp { get; set; }

        /// <summary>
        /// Side
        /// </summary>
        [JsonProperty(PropertyName = "S")]
        [Key(4)]
        public virtual AuctionSide Side { get; set; }

        /// <summary>
        /// The Price
        /// </summary>
        [JsonProperty(PropertyName = "D")]
        [Key(5)]
        public virtual double Price { get; set; }

        /// <summary>
        /// The Quantity
        /// </summary>
        [JsonProperty(PropertyName = "Q")]
        [Key(6)]
        public virtual double Quantity { get; set; }
    }
}