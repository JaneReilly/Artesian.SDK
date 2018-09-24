using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    [MessagePackObject]
    public class OperationUpdateAggregationRule : IOperationParamsPayload
    {
        /// <summary>
        /// The AggregationRule Update value
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public AggregationRule Value { get; set; }
    }
}
