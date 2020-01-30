using MessagePack;
using NodaTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The curve data for a save command.
    /// </summary>
    [MessagePackObject]
    public class UpsertCurveData
    {
        /// <summary>
        /// The default constructor
        /// </summary>
        public UpsertCurveData()
        {
        }

        /// <summary>
        /// The constructor with id
        /// </summary>
        public UpsertCurveData(MarketDataIdentifier id)
        {
            ID = id;
        }

        /// <summary>
        /// The constructor with id and version
        /// </summary>
        public UpsertCurveData(MarketDataIdentifier id, LocalDateTime version)
        {
            ID = id;
            Version = version;
        }

        /// <summary>
        /// The Market Data Identifier
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public MarketDataIdentifier ID { get; set; }

        /// <summary>
        /// The Version to operate on
        /// </summary>
        [MessagePack.Key(1)]
        public LocalDateTime? Version { get; set; }

        /// <summary>
        /// The timezone of the Rows. Must be the OriginalTimezone or, when Hourly, must be "UTC".
        /// </summary>
        [Required]
        [MessagePack.Key(2)]
        public string Timezone { get; set; }

        /// <summary>
        /// The UTC timestamp at which this assessment has been acquired/generated.
        /// </summary>
        [Required]
        [MessagePack.Key(3)]
        public Instant DownloadedAt { get; set; }

        /// <summary>
        /// The Market Data Identifier to upsert
        /// - LocalDateTime key is The Report timestamp in the MarketData OriginalTimezone but when Hourly must be UTC.
        /// - IDictionary value is The Market Data Identifier to upsert
        /// </summary>
        [MessagePack.Key(4)]
        public IDictionary<LocalDateTime, IDictionary<string, MarketAssessmentValue>> MarketAssessment { get; set; }

        /// <summary>
        /// The timeserie data in OriginalTimezone or, when Hourly, must be UTC.
        /// </summary>
        [MessagePack.Key(5)]
        public IDictionary<LocalDateTime, double?> Rows { get; set; }

        /// <summary>
        /// Flag to choose between synchronous and asynchronous command execution
        /// </summary>
        [MessagePack.Key(6)]
        public bool DeferCommandExecution { get; set; } = true;

        /// <summary>
        /// Flag to choose between synchronous and asynchronous precomputed data generation
        /// </summary>
        [MessagePack.Key(7)]
        public bool DeferDataGeneration { get; set; } = true;

        /// <summary>
        /// Flag to choose between syncronoys and asyncronous precomputed data generation
        /// </summary>
        [MessagePack.Key(8)]
        public bool KeepNulls { get; set; } = false;

        /// <summary>
        /// The timeserie data in OriginalTimezone or, when Hourly, must be UTC.
        /// </summary>
        [MessagePack.Key(9)]
        public IDictionary<LocalDateTime, AuctionBids> AuctionRows { get; set; }
    }

    internal static class UpsertCurveDataExt
    {
        public static void Validate(this UpsertCurveData upsertCurveData)
        {
            if (upsertCurveData.ID == null)
                throw new ArgumentException("UpsertCurveData ID must be valorized");

            if (upsertCurveData.Timezone != null && DateTimeZoneProviders.Tzdb.GetZoneOrNull(upsertCurveData.Timezone) == null)
                throw new ArgumentException("UpsertCurveData Timezone must be in IANA database if valorized");

            if (upsertCurveData.DownloadedAt == null)
                throw new ArgumentException("UpsertCurveData DownloadedAt must be valorized");

            if (upsertCurveData.MarketAssessment == null && (upsertCurveData.Rows == null || upsertCurveData.Rows.Count == 0) && (upsertCurveData.AuctionRows == null || upsertCurveData.AuctionRows.Count == 0))
                throw new ArgumentException("UpsertCurveData Rows/Bids must be valorized if MarketAssessment is null");

            if (upsertCurveData.MarketAssessment != null && upsertCurveData.Rows != null)
                throw new ArgumentException("UpsertCurveData MarketAssessment must be valorized if Rows are null");

            if (upsertCurveData.Rows == null)
            {
                if (upsertCurveData.Version != null)
                    throw new ArgumentException("UpsertCurveData Version must be NULL if Rows are NULL");

                if ((upsertCurveData.MarketAssessment == null || upsertCurveData.MarketAssessment.Count == 0) && (upsertCurveData.AuctionRows == null || upsertCurveData.AuctionRows.Count == 0))
                    throw new ArgumentException("UpsertCurveData MarketAssessment/Bids must be valorized if Rows are NULL");
            }
            else if (upsertCurveData.Rows != null)
            {
                if (upsertCurveData.MarketAssessment != null)
                    throw new ArgumentException("UpsertCurveData MarketAssessment must be NULL if Rows are Valorized");

                if (upsertCurveData.AuctionRows != null)
                    throw new ArgumentException("UpsertCurveData Bids must be NULL if Rows are Valorized");

                foreach (var row in upsertCurveData.Rows)
                {
                    if (row.Key == default)
                        throw new ArgumentException($"Rows[{row}]", "Invalid timepoint");
                }
            }

            if (upsertCurveData.MarketAssessment == null)
            {
                if ((upsertCurveData.Rows == null || upsertCurveData.Rows.Count == 0) && (upsertCurveData.AuctionRows == null || upsertCurveData.AuctionRows.Count == 0))
                    throw new ArgumentException("UpsertCurveData Rows/Bids must be valorized if MarketAssesment are NULL");
            }
            else if (upsertCurveData.MarketAssessment != null)
            {
                if (upsertCurveData.Rows != null)
                    throw new ArgumentException("UpsertCurveData Rows must be NULL if MarketAssessment are Valorized");

                if (upsertCurveData.AuctionRows != null)
                    throw new ArgumentException("UpsertCurveData Bids must be NULL if MarketAssessment are Valorized");
            }

            if (upsertCurveData.AuctionRows == null)
            {
                if ((upsertCurveData.Rows == null || upsertCurveData.Rows.Count == 0) && (upsertCurveData.MarketAssessment == null || upsertCurveData.MarketAssessment.Count == 0))
                    throw new ArgumentException("UpsertCurveData Rows/MarketAssessment must be valorized if Bids are NULL");
            }
            else if (upsertCurveData.AuctionRows != null)
            {
                if (upsertCurveData.Rows != null)
                    throw new ArgumentException("UpsertCurveData Rows must be NULL if Bids are Valorized");

                if (upsertCurveData.MarketAssessment != null)
                    throw new ArgumentException("UpsertCurveData MarketAssesment must be NULL if Bids are Valorized");
            }
        }
    }
}