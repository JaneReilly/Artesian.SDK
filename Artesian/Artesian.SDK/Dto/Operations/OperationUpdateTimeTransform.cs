using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    [MessagePackObject]
    public class OperationUpdateTimeTransform : IOperationParamsPayload
    {
        [Required]
        [MessagePack.Key(0)]
        public int? Value { get; set; }
    }
}
