﻿using MessagePack;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Principals entity
    /// </summary>
    [MessagePackObject]
    public class Principals
    {
        /// <summary>
        /// The Principals name
        /// </summary>
        [Key("Principal")]
        public string Principal { get; set; }
        /// <summary>
        /// The Principals type
        /// </summary>
        [Key("Type")]
        public string Type { get; set; }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
