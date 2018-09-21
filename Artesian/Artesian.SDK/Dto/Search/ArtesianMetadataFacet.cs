// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    [MessagePackObject]
    public class ArtesianMetadataFacet
    {
        /// <summary>
        /// Facet Name
        /// </summary>
        [Key(0)]
        public string FacetName { get; set; }
        /// <summary>
        /// Facet Type
        /// </summary>
        [Key(1)]
        public ArtesianMetadataFacetType FacetType { get; set; }
        /// <summary>
        /// Facet Values
        /// </summary>
        [Key(2)]
        public List<ArtesianMetadataFacetCount> Values { get; set; }
    }

    [MessagePackObject]
    public class ArtesianMetadataFacetCount
    {
        [Key(0)]
        public string Value { get; set; }
        [Key(1)]
        public long? Count { get; set; }
    }

    public enum ArtesianMetadataFacetType
    {
        Property = 0
        , Tag
    }
}
