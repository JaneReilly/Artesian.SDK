using Artesian.SDK.Dto;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// Add TimeSerie OperationResult enums
    /// </summary>
    public enum AddTimeSerieOperationResult
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ValueAdded = 0
      , TimeAlreadyPresent = 1
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Add Assessment OperationResult enums
    /// </summary>
    public enum AddAssessmentOperationResult
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        AssessmentAdded = 0
      , ProductAlreadyPresent = 1
      , IllegalReferenceDate = 2
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// Market Data Interface
    /// </summary>
    public interface IMarketData
    {
        /// <summary>
        /// MarketData Id
        /// </summary>
        int MarketDataId { get; }
        /// <summary>
        /// MarketData Identifier
        /// </summary>
        MarketDataIdentifier Identifier { get; }
        /// <summary>
        /// MarketData DataTimezone
        /// </summary>
        string DataTimezone { get; }
        /// <summary>
        /// MarketData Type
        /// </summary>
        MarketDataType? Type { get; }
        /// <summary>
        /// MarketData Granularity
        /// </summary>
        Granularity Granularity { get; }
        /// <summary>
        /// MarketData Timezone
        /// </summary>
        string Timezone { get; }
        /// <summary>
        /// MarketData Tags
        /// </summary>
        Dictionary<string, List<string>> Tags { get; }


        /// <summary>
        /// MarketData Load Metadata
        /// </summary>
        /// <remarks>
        /// Update the MarketData 
        /// </remarks>
        /// <returns></returns>
        MarketDataEntity.Input LoadMetadata();
        /// <summary>
        /// MarketData Update
        /// </summary>
        /// <remarks>
        /// Update the MarketData 
        /// </remarks>
        /// <returns></returns>
        Task Update(MarketDataEntity.Input metadata);
        /// <summary>
        /// MarketData Register
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <param name="metadata">the entity of metadata</param>
        /// <returns></returns>
        Task Register(MarketDataEntity.Input metadata);
        /// <summary>
        /// MarketData IsRegister
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <returns> Marketdata if true, null and false if not found </returns>
        Task<(MarketDataEntity.Output, bool)> IsRegistered();
    }

}
