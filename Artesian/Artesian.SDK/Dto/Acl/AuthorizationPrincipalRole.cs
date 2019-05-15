using MessagePack;
using System;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Principal Type Enum
    /// </summary>
    public enum PrincipalType
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Group,
        User
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// The Principal Entity
    /// </summary>
    [MessagePackObject]
    public class Principal : IEquatable<Principal>
    {
        /// <summary>
        /// The Principal Type
        /// </summary>
        [Key(0)]
        public PrincipalType PrincipalType { get; set; }
        /// <summary>
        /// The Principal Identifier
        /// </summary>
        [Key(1)]
        public string PrincipalId { get; set; }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(Principal other)
        {
            return PrincipalType == other.PrincipalType && PrincipalId == other.PrincipalId;
        }

        public override bool Equals(object obj)
        {
            return obj is Principal p && this.Equals(p);
        }

        public Principal()
        {

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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// The AuthorizationPrincipalRole Entity
    /// </summary>
    [MessagePackObject]
    public class AuthorizationPrincipalRole
    {
        /// <summary>
        /// The Role
        /// </summary>
        [Key(0)]
        public string Role { get; set; }
        /// <summary>
        /// The Principal
        /// </summary>
        [Key(1)]
        public Principal Principal { get; set; }
        /// <summary>
        /// The information regarding Inheritance
        /// </summary>
        [Key(2)]
        public string InheritedFrom { get; set; }

        /// <summary>
        /// Encode principal Enum
        /// </summary>
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

        /// <summary>
        /// Decode principal Enum
        /// </summary>
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

