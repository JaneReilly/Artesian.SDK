using MessagePack;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Operations to be executed on a list of ids
    /// </summary>
    [MessagePackObject]
    public class Operations
    {
        public Operations()
        {
            IDS = new HashSet<MarketDataETag>();
            OperationList = new List<OperationParams>();
        }

        /// <summary>
        /// The Market Data Identifiers
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public ISet<MarketDataETag> IDS { get; set; }

        /// <summary>
        /// The Operations to be executed
        /// </summary>
        [Required]
        [MessagePack.Key(1)]
        public IList<OperationParams> OperationList { get; set; }
    }

}
