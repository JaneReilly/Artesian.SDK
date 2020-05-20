using System;
using System.Collections.Generic;
using System.Text;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// Purpose
    /// </summary>
    public enum Status : byte
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ACC = 0,
        REJ = 1,
        INC = 2,
        REP = 3,
        REV = 4,
        SUB = 5,
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    }
}
