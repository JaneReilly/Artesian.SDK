using Artesian.SDK.Dto;
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
        /// <summary>
        /// MarketData metadata service
        /// </summary>
        protected IMetadataService _metadataService;

        /// <summary>
        /// MarketData Id
        /// </summary>
        public int? MarketDataId { get { return Entity?.MarketDataId; } }

        /// <summary>
        /// MarketData Identifier
        /// </summary>
        public MarketDataIdentifier Identifier { get; protected set; }

        /// <summary>
        /// MarketData Entity
        /// </summary>
        public ReadOnlyMarketDataEntity Entity { get; protected set; }

        /// <summary>
        /// MarketData entity
        /// </summary>
        protected MarketDataEntity.Output _entity = null;

        /// <summary>
        /// MarketData Constructor by Id
        /// </summary>
        public MarketData(IMetadataService metadataService, MarketDataIdentifier id)
        {
            EnsureArg.IsNotNull(metadataService, nameof(metadataService));

            _metadataService = metadataService;

            Identifier = id;
        }

        /// <summary>
        /// MarketData Constructor
        /// </summary>
        public MarketData(IMetadataService metadataService, MarketDataEntity.Output entity)
        {
            EnsureArg.IsNotNull(metadataService, nameof(metadataService));

            _metadataService = metadataService;
            _create(entity);
        }

        private void _create(MarketDataEntity.Output entity)
        {
            Identifier = new MarketDataIdentifier(entity.ProviderName, entity.MarketDataName);
            Entity = new ReadOnlyMarketDataEntity(entity);
            _entity = entity;
        }

        /// <summary>
        /// MarketData Register
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <param name="metadata">the entity of metadata</param>
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
                throw new ActualTimeSerieException("Actual Time Serie is already registered with ID {0}", _entity.MarketDataId);

            _entity = await _metadataService.RegisterMarketDataAsync(metadata, ctk);

            Entity = new ReadOnlyMarketDataEntity(_entity);
        }

        /// <summary>
        /// MarketData IsRegister
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <returns> true if  Marketdata si present, false if not found </returns>
        public async Task<bool> IsRegistered(CancellationToken ctk = default)
        {
            if (_entity == null)
                _entity = await _metadataService.ReadMarketDataRegistryAsync(this.Identifier, ctk);

            if (_entity != null)
            {
                Entity = new ReadOnlyMarketDataEntity(_entity);
                return true;
            }
                
            return false;
        }

        /// <summary>
        /// Loads MarketData Metadata
        /// </summary>
        /// <remarks>
        /// Loads MarketData Metadata
        /// </remarks>
        /// <returns></returns>
        public async Task LoadMetadata(CancellationToken ctk = default)
        {
            if (_entity == null)
                _entity = await _metadataService.ReadMarketDataRegistryAsync(this.Identifier, ctk);

            if (_entity != null)
                Entity = new ReadOnlyMarketDataEntity(_entity);
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
                throw new ActualTimeSerieException("Actual Time Serie is not yet registered");

            var metadata = _entity;
            _entity = await _metadataService.UpdateMarketDataAsync(metadata, ctk);

            Entity = new ReadOnlyMarketDataEntity(_entity);
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
                throw new ActualTimeSerieException("Actual Time Serie is not yet registered");

            if (_entity.Type != MarketDataType.ActualTimeSerie)
                throw new MarketAssessmentException("Entity is not Actual Time Serie");

            var actual = new ActualTimeSerie(_metadataService, _entity);
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
                throw new VersionedTimeSerieException("Versioned Time Serie is not yet registered");

            if (_entity.Type != MarketDataType.VersionedTimeSerie)
                throw new MarketAssessmentException("Entity is not Versioned Time Serie");

            var versioned = new VersionedTimeSerie(_metadataService, _entity);
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
                throw new MarketAssessmentException("Entity is not Market Assessement");

            var mas = new MarketAssessment(_metadataService, _entity);
            return mas;
        }
    }

}
