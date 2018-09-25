using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Operation for Update Time Transform
    /// </summary>
    [MessagePackObject]
    public class OperationUpdateTimeTransform : IOperationParamsPayload
    {
        /// <summary>
        /// The Time Transform Update value
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public int? Value { get; set; }
    }
}
