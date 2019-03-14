// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime.Text;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Query Paramaters DTO
    /// </summary>
    public  abstract class QueryParamaters
    {
        /// <summary>
        /// IDs
        /// </summary>
        public IEnumerable<int> Ids;
        /// <summary>
        /// Extraction range config
        /// </summary>
        public ExtractionRangeSelectionConfig ExtractionRangeCfg;
        /// <summary>
        /// Extraction range type
        /// </summary>
        public ExtractionRangeType? ExtractionRangeType;
        /// <summary>
        /// 
        /// </summary>
        private static LocalDatePattern LocalDatePattern = LocalDatePattern.Iso;
        private static LocalDateTimePattern LocalDateTimePattern = LocalDateTimePattern.ExtendedIso;

    }
}
