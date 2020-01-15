using MessagePack;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artesian.SDK.Dto.PublicOffer
{
    /// <summary>
    /// PublicOfferCurve class
    /// </summary>
    [MessagePackObject]
    public class PublicOfferCurve
    {
        /// <summary>
        /// The Status
        /// </summary>
        [Key(0)]
        public Status Status { get; set; }
        /// <summary>
        /// The BAType
        /// </summary>
        [Key(1)]
        public BAType BAType { get; set; }
        /// <summary>
        /// The Scope
        /// </summary>
        [Key(2)]
        public Scope Scope { get; set; }
        /// <summary>
        /// The Date
        /// </summary>
        [Key(3)]
        public LocalDate Date { get; set; }
        /// <summary>
        /// The HourN
        /// </summary>
        [Key(4)]
        public int HourN { get; set; }
        /// <summary>
        /// The Market
        /// </summary>
        [Key(5)]
        public Market Market { get; set; }
        /// <summary>
        /// The Purpose
        /// </summary>
        [Key(6)]
        public Purpose Purpose { get; set; }
        /// <summary>
        /// The Zone
        /// </summary>
        [Key(7)]
        public Zone Zone { get; set; }
        /// <summary>
        /// The UnitType
        /// </summary>
        [Key(8)]
        public UnitType UnitType { get; set; }
        /// <summary>
        /// The FuelType
        /// </summary>
        [Key(9)]
        public GenerationType FuelType { get; set; }
        /// <summary>
        /// The Unit
        /// </summary>
        [Key(10)]
        public string Unit { get; set; }
        /// <summary>
        /// The Operator
        /// </summary>
        [Key(11)]
        public string Operator { get; set; }
        /// <summary>
        /// The AwardedQuantity
        /// </summary>
        [Key(12)]
        public decimal? AwardedQuantity { get; set; }
    }
}
