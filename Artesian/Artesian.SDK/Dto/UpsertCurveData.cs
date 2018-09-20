using MessagePack;
using NodaTime;
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

        public UpsertCurveData()
        {
        }

        public UpsertCurveData(MarketDataIdentifier id)
        {
            ID = id;
        }

        public UpsertCurveData(MarketDataIdentifier id, LocalDateTime version )
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
        /// - LocalDateTime key is The Report timestamp in the MarketData OriginalTimezone but UTC when Hourly.
        /// - IDictionary value is The Market Data Identifier to upsert
        /// </summary>
        [MessagePack.Key(4)]
        public IDictionary<LocalDateTime, IDictionary<string, MarketAssessmentValue>> MarketAssessment { get; set; }
        // public IDictionary<string, MarketAssessmentValue> MarketAssessment {get; set; }

        /// <summary>
        /// The timeserie data in OriginalTimezone or, when Hourly, UTC.
        /// </summary>
        [MessagePack.Key(5)]
        public IDictionary<LocalDateTime, double?> Rows { get; set; }


        /// <summary>
        /// Flag to choose between syncronoys and asyncronous command execution
        /// </summary>
        [MessagePack.Key(6)]
        public bool DeferCommandExecution { get; set; } = true;

        /// <summary>
        /// Flag to choose between syncronoys and asyncronous precomputed data generation
        /// </summary>
        [MessagePack.Key(7)]
        public bool DeferDataGeneration { get; set; } = true;
    }

    //public class UpsertCurveDataExt
    //{
    //    public UpsertCurveDataValidator()
    //    {
    //        RuleFor(x => x.ID)
    //            .NotNull()
    //            .SetValidator(new MarketDataIdentifierValidator())
    //            ;

    //        RuleFor(x => x.Timezone)
    //            .Must(x => DateTimeZoneProviders.Tzdb.GetZoneOrNull(x) != null)
    //            .When(x => x.Timezone != null)
    //            .WithMessage("Timezone is not present in IANA database")
    //            ;

    //        RuleFor(x => x.DownloadedAt)
    //            .NotEmpty()
    //            ;

    //        When(x => x.MarketAssessment == null, () =>
    //         {
    //             RuleFor(x => x.Rows)
    //                 .NotEmpty()
    //                 ;
    //         });

    //         When(x => x.MarketAssessment != null, () =>
    //         {
    //             RuleFor(x => x.Rows)
    //                 .Null()
    //                 ;
    //         });

    //        When(x => x.Rows == null, () =>
    //        {
    //            RuleFor(x => x.Version)
    //                .Null()
    //                ;

    //            RuleFor(x => x.MarketAssessment)
    //                .NotEmpty()
    //                ;

    //            RuleForEach(x => x.MarketAssessment)
    //                .Custom((x, ctx) =>
    //                {
    //                    IMarketProduct dummy;
    //                        foreach (var i in x.Value)
    //                        {
    //                            if (!MarketProductBuilder.TryParse(i.Key, out dummy))
    //                                ctx.AddFailure(new FluentValidation.Results.ValidationFailure($"MarketAssessment.{x.Key}", "Invalid product string", i.Key));
    //                        }
    //                });
    //        });

    //        When(x => x.Rows != null, () =>
    //        {
    //            RuleFor(x => x.MarketAssessment)
    //                .Null()
    //                ;

    //            RuleForEach(x => x.Rows)
    //                .Custom((x, ctx) =>
    //                {
    //                    if (x.Key == default)
    //                       ctx.AddFailure(new FluentValidation.Results.ValidationFailure($"Rows[{x}]", "Invalid timepoint", x));
    //                });
    //        });

    //    }
    //}
}
