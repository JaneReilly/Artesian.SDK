using MessagePack;
using System;

namespace Artesian.SDK.Dto
{
    public enum PrincipalType
    {
        Group,
        User
    }

    [MessagePackObject]
    public struct Principal : IEquatable<Principal>
    {
        [Key(0)]
        public PrincipalType PrincipalType { get; set; }
        [Key(1)]
        public string PrincipalId { get; set; }

        public bool Equals(Principal other)
        {
            return PrincipalType == other.PrincipalType && PrincipalId == other.PrincipalId;
        }

        public override bool Equals(object obj)
        {
            return obj is Principal p && this.Equals(p);
        }

        public Principal(string s)
        {
            PrincipalId = s.Substring(2);
            PrincipalType = AuthorizationPrincipalRole.DecodePrincipalEnum(s.Substring(0, 1));
        }

        public override string ToString()
        {
            return $"{AuthorizationPrincipalRole.EncodePrincipalEnum(PrincipalType)}:{PrincipalId}";
        }

        public override int GetHashCode()
        {
            var hashCode = -109350059;
            hashCode = hashCode * -1521134295 + PrincipalType.GetHashCode();
            hashCode = hashCode * -1521134295 + PrincipalId.GetHashCode();
            return hashCode;
        }

        public static implicit operator string(Principal url) { return url.ToString(); }

        public static implicit operator Principal(string url) { return new Principal(url); }
    }

    [MessagePackObject]
    public class AuthorizationPrincipalRole
    {
        [Key(0)]
        public string Role { get; set; }
        [Key(1)]
        public Principal Principal { get; set; }
        [Key(2)]
        public string InheritedFrom { get; set; }

        public static string EncodePrincipalEnum(PrincipalType principalEnum)
        {
            switch (principalEnum)
            {
                case PrincipalType.Group:
                    return "g";
                case PrincipalType.User:
                    return "u";
            }

            throw new InvalidOperationException("unexpected PrincipalType");
        }

        public static PrincipalType DecodePrincipalEnum(string encoded)
        {
            switch (encoded)
            {
                case "g":
                    return PrincipalType.Group;
                case "u":
                    return PrincipalType.User;
            }

            throw new InvalidOperationException("unexpected encoded string for PrincipalType");
        }
    }
}

