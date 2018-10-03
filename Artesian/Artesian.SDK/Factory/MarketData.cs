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
        /// MarketData entity
        /// </summary>
        protected MarketDataEntity.Output _entity = null;
        /// <summary>
        /// MarketData metadata
        /// </summary>
        protected MarketDataEntity.Input _metadata = null;

        /// <summary>
        /// MarketData Id
        /// </summary>
        public int MarketDataId { get { return _entity == null ? 0 : _entity.MarketDataId; } }

        /// <summary>
        /// MarketData Identifier
        /// </summary>
        public MarketDataIdentifier Identifier { get; protected set; }

        /// <summary>
        /// MarketData DataTimezone
        /// </summary>
        public string DataTimezone
        {
            get
            {
                if (_entity?.OriginalGranularity.IsTimeGranularity() == true)
                    return "UTC";
                else
                    return _entity?.OriginalTimezone;
            }
        }

        /// <summary>
        /// MarketData Type
        /// </summary>
        public MarketDataType? Type { get { return _entity?.Type; } }

        /// <summary>
        /// MarketData Granularity
        /// </summary>
        public Granularity Granularity { get { return _entity == null ? default : _entity.OriginalGranularity; } }

        /// <summary>
        /// MarketData Timezone
        /// </summary>
        public string Timezone => _entity?.OriginalTimezone;

        /// <summary>
        /// MarketData Tags
        /// </summary>
        public Dictionary<string, List<string>> Tags { get { return _entity?.Tags; } }

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
            _entity = entity;
        }

        #region Interface Methods
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
        }
        /// <summary>
        /// MarketData IsRegister
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <returns> Marketdata if true, null and false if not found </returns>
        public async Task<(MarketDataEntity.Output, bool)> IsRegistered()
        {
            if (_entity == null)
                _entity = await _metadataService.ReadMarketDataRegistryAsync(this.Identifier);

            if (_entity != null)
                return (_entity, true);

            return (null, false);
        }
        /// <summary>
        /// MarketData Load Metadata
        /// </summary>
        /// <remarks>
        /// Update the MarketData 
        /// </remarks>
        /// <returns></returns>
        public MarketDataEntity.Input LoadMetadata()
        {
            if (_entity == null)
                throw new ActualTimeSerieException("Actual Time Serie is not yet registered");

            _metadata = _entity;

            return _metadata;
        }
        /// <summary>
        /// MarketData Update
        /// </summary>
        /// <remarks>
        /// Update the MarketData 
        /// </remarks>
        /// <returns></returns>
        public async Task Update(MarketDataEntity.Input metadata, CancellationToken ctk = default)
        {
            if (_entity == null)
                throw new ActualTimeSerieException("Actual Time Serie is not yet registered");

            _entity = await _metadataService.UpdateMarketDataAsync(metadata, ctk);
        }

        /// <summary>
        /// MarketData Edit
        /// </summary>
        /// <remarks>
        /// Start write mode for MarketData
        /// </remarks>
        /// <returns> IMarketData </returns>
        public IMarketData Edit()
        {
            if (_entity == null)
                throw new ActualTimeSerieException("Actual Time Serie is not yet registered");

            switch (_entity.Type)
            {
                case MarketDataType.ActualTimeSerie:
                    {
                        var actual = new ActualTimeSerie(_metadataService, _entity);
                        return actual;
                    }
                case MarketDataType.MarketAssessment:
                    {
                        var marketAssessment = new MarketAssessment(_metadataService, _entity);
                        return marketAssessment;
                    }
                case MarketDataType.VersionedTimeSerie:
                    {
                        var versioned = new VersionedTimeSerie(_metadataService, _entity);
                        return versioned;
                    }
                default:
                    throw new NotSupportedException($"The Type '{_entity.Type}'is not present");
            }
        }
        #endregion
    }

}
