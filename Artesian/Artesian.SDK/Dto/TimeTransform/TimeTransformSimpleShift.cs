// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using System;

namespace Artesian.SDK.Dto
{
    [MessagePackObject]
    public class TimeTransformSimpleShift : TimeTransform
    {
        [Key("Period")]
        public Granularity Period { get; set; }
        [Key(">")]
        public string PositiveShift { get; set; }
        [Key("<")]
        public string NegativeShift { get; set; }

        [IgnoreMember]
        public override TransformType Type => TransformType.SimpleShift;
    }

    public static class TimeTransformSimpleShiftExt
    {
        public static void Validate(this TimeTransformSimpleShift timeTransform)
        {
            if (String.IsNullOrWhiteSpace(timeTransform.Name))
                throw new ArgumentException("timeTransform Name must be valorized");

            //if (Enum.TryParse<Granularity>(timeTransform.Period, out var res) == false)

            if (String.IsNullOrWhiteSpace(timeTransform.NegativeShift) && String.IsNullOrWhiteSpace(timeTransform.PositiveShift))
                throw new ArgumentException("At least one between positive or negative shift must be valorized");
        }
    }
}
