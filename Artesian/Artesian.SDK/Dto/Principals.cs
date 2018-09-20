using MessagePack;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    [MessagePackObject]
    public class Principals
    {
        [Key("Principal")]
        public string Principal { get; set; }
        [Key("Type")]
        public string Type { get; set; }

        public override bool Equals(object obj)
        {
            var principals = obj as Principals;
            return principals != null &&
                   Principal == principals.Principal &&
                   Type == principals.Type;
        }

        public override int GetHashCode()
        {
            var hashCode = 2064342430;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Principal);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
            return hashCode;
        }
    }
}
