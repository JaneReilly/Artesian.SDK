using Artesian.SDK.Dto;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// Market Data Interface
    /// </summary>
    public interface IMarketData
    {
        /// <summary>
        /// MarketData Id
        /// </summary>
        int? MarketDataId { get; }
        /// <summary>
        /// MarketData Identifier
        /// </summary>
        MarketDataIdentifier Identifier { get; }
        /// <summary>
        /// MarketData ReadOnly Entity
        /// </summary>
        MarketDataMetadata Metadata { get; }

        /// <summary>
        /// MarketData Load Metadata
        /// </summary>
        /// <remarks>
        /// Update the MarketData 
        /// </remarks>
        /// <returns></returns>
        Task Load(CancellationToken ctk = default);
        /// <summary>
        /// MarketData Update
        /// </summary>
        /// <remarks>
        /// Update the MarketData 
        /// </remarks>
        /// <param name="ctk">Cancellation token</param>
        /// <returns></returns>
        Task Update(CancellationToken ctk = default);
        /// <summary>
        /// MarketData Register
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <param name="metadata">the entity of metadata</param>
        /// <param name="ctk">Cancellation token</param>
        /// <returns></returns>
        Task Register(MarketDataEntity.Input metadata, CancellationToken ctk = default);
        /// <summary>
        /// MarketData IsRegister
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <returns> Marketdata if true, null and false if not found </returns>
        Task<bool> IsRegistered(CancellationToken ctk = default);

        /// <summary>
        /// Edit for Actual Timeserie
        /// </summary>
        /// <remarks>
        /// Start write mode for Actual Timeserie
        /// </remarks>
        /// <returns> Marketdata </returns>
        ITimeserieWritable EditActual();
        /// <summary>
        /// Edit for Versioned Timeserie
        /// </summary>
        /// <remarks>
        /// Start write mode for Versioned Timeserie
        /// </remarks>
        /// <returns> Marketdata </returns>
        ITimeserieWritable EditVersioned(LocalDateTime version);
        /// <summary>
        /// Edit for Market Assessment
        /// </summary>
        /// <remarks>
        /// Start write mode for Market Assessment
        /// </remarks>
        /// <returns> Marketdata </returns>
        IMarketAssessmentWritable EditMarketAssessment();
    }

    /// <summary>
    /// Interface for Market Assessment Write
    /// </summary>
    public interface IMarketAssessmentWritable
    {
        /// <summary>
        /// MarketAssessment AddData
        /// </summary>
        /// <remarks>
        /// MarketAssessment AddData
        /// </remarks>
        /// <param name="localDate">the local date of the value</param>
        /// <param name="product">the product</param>
        /// <param name="value">Market assessment Value</param>
        /// <returns></returns>
        AddAssessmentOperationResult AddData(LocalDate localDate, string product, MarketAssessmentValue value);
        /// <summary>
        /// MarketAssessment AddData
        /// </summary>
        /// <remarks>
        /// MarketAssessment AddData
        /// </remarks>
        /// <param name="time">the istant of the value</param>
        /// <param name="product">the product</param>
        /// <param name="value">Market assessment Value</param>
        /// <returns></returns>
        AddAssessmentOperationResult AddData(Instant time, string product, MarketAssessmentValue value);
        /// <summary>
        /// MarketAssessment ClearData
        /// </summary>
        void ClearData();
        /// <summary>
        /// MarketAssessment Save
        /// </summary>
        /// <remarks>
        /// MarketAssessment Save
        /// </remarks>
        /// <param name="downloadedAt">the istant downloaded</param>
        /// <param name="deferCommandExecution">defer Command Execution</param>
        /// <param name="deferDataGeneration">defer Data Generation</param>
        /// <returns></returns>
        Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true);
    }

    /// <summary>
    /// Interface for timeserie write
    /// </summary>
    /// <remarks>
    /// Common for Actual and Versioned timeserie
    /// </remarks>
    /// <returns> Marketdata </returns>
    public interface ITimeserieWritable
    {
        /// <summary>
        /// TimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on curve with localDate
        /// </remarks>
        /// <param name="localDate">the local date of the value</param>
        /// <param name="value">value</param>
        /// <returns>AddTimeSerieOperationResult</returns>
        AddTimeSerieOperationResult AddData(LocalDate localDate, double? value);
        /// <summary>
        /// TimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on curve with Instant
        /// </remarks>
        /// <param name="time">the istant of the value</param>
        /// <param name="value">value</param>
        /// <returns>AddTimeSerieOperationResult</returns>
        AddTimeSerieOperationResult AddData(Instant time, double? value);
        /// <summary>
        /// TimeSerie ClearData
        /// </summary>
        /// <remarks>
        /// Clear all the data sey in the Values
        /// </remarks>
        /// <returns></returns>
        void ClearData();
        /// <summary>
        /// TimeSerie Save
        /// </summary>
        /// <remarks>
        /// TimeSerie Save
        /// </remarks>
        /// <param name="downloadedAt">the istant downloaded</param>
        /// <param name="deferCommandExecution">defer Command Execution</param>
        /// <param name="deferDataGeneration">defer Data Generation</param>
        /// <returns></returns>
        Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true);
    }
}
