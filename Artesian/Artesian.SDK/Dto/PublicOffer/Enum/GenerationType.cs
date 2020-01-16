using System;
using System.Collections.Generic;
using System.Text;

namespace Artesian.SDK.Dto.PublicOffer
{
    /// <summary>
    /// Purpose
    /// </summary>
    public enum GenerationType : byte
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        UNKNOWN = 0,
        OTHER = 1,
        AUTOGENERATION = 2,
        BIOMASS = 3,
        COAL = 4,
        WIND = 5,
        PV = 6,
        GAS = 7,
        GASOIL = 8,
        THERMAL = 9,
        HYDRO = 10,
        MIXED = 11,
        OIL = 12
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    }
}
