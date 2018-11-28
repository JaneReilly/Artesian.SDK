﻿using Artesian.SDK.Dto;
using Artesian.SDK.Service;
using EnsureThat;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// Market Data Generic class
    /// </summary>
    public class MarketData : IMarketData
    {
        internal IMarketDataService _marketDataService;
        internal MarketDataEntity.Output _entity = null;

        /// <summary>
        /// MarketData Id
        /// </summary>
        public int? MarketDataId { get { return _entity?.MarketDataId; } }

        /// <summary>
        /// MarketData Identifier
        /// </summary>
        public MarketDataIdentifier Identifier { get; protected set; }

        /// <summary>
        /// MarketData Entity
        /// </summary>
        public MarketDataMetadata Metadata { get; protected set; }

        /// <summary>
        /// MarketData Constructor by Id
        /// </summary>
        internal MarketData(IMarketDataService marketDataService, MarketDataIdentifier id)
        {
            EnsureArg.IsNotNull(marketDataService, nameof(marketDataService));

            _marketDataService = marketDataService;
          
            Identifier = id;
        }

        /// <summary>
        /// MarketData Register
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <param name="metadata">The entity of metadata</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public async Task Register(MarketDataEntity.Input metadata, CancellationToken ctk = default)
        {
            EnsureArg.IsNotNull(metadata, nameof(metadata));
            EnsureArg.IsTrue(metadata.ProviderName == null || metadata.ProviderName == this.Identifier.Provider);
            EnsureArg.IsTrue(metadata.ProviderName == null || metadata.ProviderName == this.Identifier.Provider);
            EnsureArg.IsTrue(metadata.MarketDataName == null || metadata.MarketDataName == this.Identifier.Name);
            EnsureArg.IsNotNullOrWhiteSpace(metadata.OriginalTimezone);

            metadata.ProviderName = this.Identifier.Provider;
            metadata.MarketDataName = this.Identifier.Name;

            if (_entity != null)
                throw new ActualTimeSerieException("Actual Time Series is already registered with ID {0}", _entity.MarketDataId);

            _entity = await _marketDataService.RegisterMarketDataAsync(metadata, ctk);

            Metadata = new MarketDataMetadata(_entity);
        }

        /// <summary>
        /// MarketData IsRegister
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <returns> True if  Marketdata is present, false if not found </returns>
        public async Task<bool> IsRegistered(CancellationToken ctk = default)
        {
            if (Metadata != null)
                return true;
            else
            {
                await Load(ctk);

                if (Metadata != null)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Loads MarketData Metadata
        /// </summary>
        /// <remarks>
        /// Loads MarketData Metadata
        /// </remarks>
        /// <returns></returns>
        public async Task Load(CancellationToken ctk = default)
        {
            _entity = await _marketDataService.ReadMarketDataRegistryAsync(this.Identifier, ctk);

            if (_entity != null)
                Metadata = new MarketDataMetadata(_entity);
        }

        /// <summary>
        /// MarketData Update
        /// </summary>
        /// <remarks>
        /// Update the MarketData 
        /// </remarks>
        /// <returns></returns>
        public async Task Update(CancellationToken ctk = default)
        {
            if (_entity == null)
                throw new ArtesianFactoryException("Market Data is not yet registered");

            var marketDataEntityInput = new MarketDataEntity.Input(_entity);

            _entity = await _marketDataService.UpdateMarketDataAsync(marketDataEntityInput, ctk);

            Metadata = new MarketDataMetadata(_entity);
        }

        /// <summary>
        /// Actual Timeserie Edit
        /// </summary>
        /// <remarks>
        /// Start write mode for Actual Timeserie
        /// </remarks>
        /// <returns> ITimeserieWritable </returns>
        public ITimeserieWritable EditActual()
        {
            if (_entity == null)
                throw new ActualTimeSerieException("Actual Time Series is not yet registered");

            if (_entity.Type != MarketDataType.ActualTimeSerie)
                throw new MarketAssessmentException("Entity is not an Actual Time Serie");

            var actual = new ActualTimeSerie(this);
            return actual;
        }

        /// <summary>
        /// Versioned Timeserie Edit
        /// </summary>
        /// <remarks>
        /// Start write mode for Versioned Timeserie
        /// </remarks>
        /// <returns> ITimeserieWritable </returns>
        public ITimeserieWritable EditVersioned(LocalDateTime version)
        {
            if (_entity == null)
                throw new VersionedTimeSerieException("Versioned Time Series is not yet registered");

            if (_entity.Type != MarketDataType.VersionedTimeSerie)
                throw new MarketAssessmentException("Entity is not  a Versioned Time Serie");

            var versioned = new VersionedTimeSerie(this);
            versioned.SetSelectedVersion(version);

            return versioned;
        }

        /// <summary>
        /// Market Assessment Edit
        /// </summary>
        /// <remarks>
        /// Start write mode for Market Assessment
        /// </remarks>
        /// <returns> IMarketAssessmentWritable </returns>
        public IMarketAssessmentWritable EditMarketAssessment()
        {
            if (_entity == null)
                throw new MarketAssessmentException("Market Assessement is not yet registered");

            if(_entity.Type != MarketDataType.MarketAssessment)
                throw new MarketAssessmentException("Entity is not a Market Assessement");

            var mas = new MarketAssessment(this);
            return mas;
        }
    }

}
