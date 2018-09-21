using MessagePack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Operations to be executed on a list of ids
    /// </summary>
    [MessagePackObject]
    public class Operations
    {
        public Operations()
        {
            IDS = new HashSet<MarketDataETag>();
            OperationList = new List<OperationParams>();
        }

        /// <summary>
        /// The Market Data Identifiers
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public ISet<MarketDataETag> IDS { get; set; }

        /// <summary>
        /// The Operations to be executed
        /// </summary>
        [Required]
        [MessagePack.Key(1)]
        public IList<OperationParams> OperationList { get; set; }
    }


    public static class OperationsExt
    {
        public static void Validate(this Operations operations)
        {
            foreach (var id in operations.IDS)
            {
                if (id == null)
                    throw new ArgumentException("Operations: any single ID must be valorized");
                else
                {
                    if (String.IsNullOrWhiteSpace(id.ETag))
                        throw new ArgumentException("Operations: any single ETAG must be valorized");
                }
            }

            foreach (var op in operations.OperationList)
            {
                if (op == null)
                    throw new ArgumentException("Operations any single operation must be valorized");
                else
                {
                    if (op.Params == null)
                        throw new ArgumentException("Operations: any single Params in operationList must be valorized");

                    if (op.Type == OperationType.EnableTag)
                    {
                        var p = op.Params as OperationEnableDisableTag;

                        //if (p.TagKey == "Type")
                        //    throw new ArgumentException("Operations: any single Params TagKey must have specific values");

                        //.IsValidString(3, 50)
                        //.NotEqual("Type")
                        //.NotEqual("ProviderName")
                        //.NotEqual("MarketDataName")
                        //.NotEqual("OriginalGranularity")
                        //.NotEqual("OriginalTimezone")
                        //.NotEqual("MarketDataId")
                        //.NotEqual("AggregationRule")
                    }
                    //When(x => x.Type == OperationType.EnableTag, () =>
                    //{
                    //    RuleFor(x => x.Params as OperationEnableDisableTag)
                    //        .SetValidator(new OperationEnableDisableTagValidator())
                    //        .WithName("Params")
                    //        ;
                    //});


                    //When(x => x.Type == OperationType.DisableTag, () =>
                    //{
                    //    RuleFor(x => x.Params as OperationEnableDisableTag)
                    //        .SetValidator(new OperationEnableDisableTagValidator())
                    //        .WithName("Params")
                    //        ;
                    //});


                }




                //When(x => x.Type == OperationType.UpdateTimeTransformID, () =>
                //{
                //    RuleFor(x => x.Params as OperationUpdateTimeTransform)
                //        .SetValidator(new OperationUpdateTimeTransformValidator(ctx, ids))
                //        .WithName("Params")
                //        ;
                //});

                //When(x => x.Type == OperationType.UpdateAggregationRule, () =>
                //{
                //    RuleFor(x => x.Params as OperationUpdateAggregationRule)
                //        .SetValidator(new OperationUpdateAggregationRuleValidator())
                //        .WithName("Params")
                //        ;
                //});

                //When(x => x.Type == OperationType.UpdateOriginalTimeZone, () =>
                //{
                //    RuleFor(x => x.Params as OperationUpdateOriginalTimeZone)
                //        .SetValidator(new OperationUpdateOriginalTimeZoneValidator())
                //        .WithName("Params")
                //        ;
                //});
            }
        }
    }



}
