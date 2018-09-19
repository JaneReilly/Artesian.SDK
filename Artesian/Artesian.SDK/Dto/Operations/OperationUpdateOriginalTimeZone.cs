using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    [MessagePackObject]
    public class OperationUpdateOriginalTimeZone : IOperationParamsPayload
    {
        [Required]
        [MessagePack.Key(0)]
        public string Value { get; set; }
    }
}
