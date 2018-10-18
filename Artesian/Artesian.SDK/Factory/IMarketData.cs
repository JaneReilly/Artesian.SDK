using Artesian.SDK.Dto;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
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
        /// MarketData Entity
        /// </summary>
        ReadOnlyMarketDataEntity Entity { get; }
        /// <summary>
        /// MarketData Load Metadata
        /// </summary>
        /// <remarks>
        /// Update the MarketData 
        /// </remarks>
        /// <returns></returns>
        Task LoadMetadata(CancellationToken ctk = default);
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

    /// <summary>
    /// Read Only Class for MarketData Entity
    /// </summary>
    public class ReadOnlyMarketDataEntity
    {
        private MarketDataEntity.Output _output;

        /// <summary>
        /// Read Only Class for MarketData Entity default constructor
        /// </summary>
        public ReadOnlyMarketDataEntity()
        {
        }

        /// <summary>
        /// Read Only Class for MarketData Entity constructor
        /// </summary>
        public ReadOnlyMarketDataEntity(MarketDataEntity.Output output)
        {
            _output = output;
        }

        /// <summary>
        /// The Market Data Identifier
        /// </summary>
        public int MarketDataId => _output.MarketDataId;
        /// <summary>
        /// The Market Data Etag
        /// </summary>
        public string ETag => _output.ETag;
        /// <summary>
        /// The Market Data Provider Name
        /// </summary>
        public string ProviderName => _output.ProviderName;
        /// <summary>
        /// The Market Data Name
        /// </summary>
        public string MarketDataName => _output.MarketDataName;
        /// <summary>
        /// The Original Granularity
        /// </summary>
        public Granularity? OriginalGranularity => _output?.OriginalGranularity; //Nullable Added
        /// <summary>
        /// The Type
        /// </summary>
        public MarketDataType? Type => _output?.Type; //Nullable Added
        /// <summary>
        /// The Original Timezone
        /// </summary>
        public string OriginalTimezone => _output.OriginalTimezone;
        /// <summary>
        /// The Aggregation Rule
        /// </summary>
        public AggregationRule? AggregationRule => _output?.AggregationRule; //Nullable Added
        /// <summary>
        /// The TimeTransformID
        /// </summary>
        public int? TransformID => _output.TransformID;
        /// <summary>
        /// The Provider description
        /// </summary>
        public string ProviderDescription => _output.ProviderDescription;
        /// <summary>
        /// The custom Tags assigned to the data
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyList<string>> Tags => (IReadOnlyDictionary<string, IReadOnlyList<string>>)_output.Tags.ToDictionary(pair => pair.Key, pair => pair.Value.AsReadOnly());
        /// <summary>
        /// The Authorization Path
        /// </summary>
        public string Path => _output.Path;
        /// <summary>
        /// The TimeTransform
        /// </summary>
        public TimeTransform Transform => _output.Transform;
        /// <summary>
        /// The Last time the metadata has been updated
        /// </summary>
        public Instant LastUpdated => _output.LastUpdated;
        /// <summary>
        /// The Last time the data has been writed
        /// </summary>
        public Instant? DataLastWritedAt => _output.DataLastWritedAt;
        /// <summary>
        /// Date start of range for this curve  
        /// </summary>
        public LocalDate? DataRangeStart => _output.DataRangeStart;
        /// <summary>
        /// Date end of range for this curve  
        /// </summary>
        public LocalDate? DataRangeEnd => _output.DataRangeEnd;
        /// <summary>
        /// The time the market data has been created
        /// </summary>
        public Instant Created => _output.Created;
    }

}
