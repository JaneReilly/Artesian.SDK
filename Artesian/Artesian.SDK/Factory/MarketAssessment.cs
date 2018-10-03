using Artesian.SDK.Common;
using Artesian.SDK.Dto;
using Artesian.SDK.Service;
using EnsureThat;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// MarketAssessment entity
    /// </summary>
    public class MarketAssessment : IMarketData
    {
        //private static Logger _logger = LogManager.GetCurrentClassLogger();

        //Class members
        private readonly IMetadataService _metadataService;
        private MarketDataEntity.Output _entity = null;
        private MarketDataEntity.Input _metadata = null;

        private bool _isInWriteMode = false;

        /// <summary>
        /// MarketData AssessmentElement
        /// </summary>
        public List<AssessmentElement> Assessments { get; protected set; }

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
        public MarketDataType Type => MarketDataType.MarketAssessment;

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
        /// MarketAssessment Constructor
        /// </summary>
        public MarketAssessment(IMetadataService metadataService, MarketDataEntity.Output entity)
        {
            EnsureArg.IsNotNull(metadataService, nameof(metadataService));

            _metadataService = metadataService;
            _create(entity);

            Assessments = new List<AssessmentElement>();
        }

        private void _create(MarketDataEntity.Output entity)
        {
            Identifier = new MarketDataIdentifier(entity.ProviderName, entity.MarketDataName);
            _entity = entity;
        }

        #region Interface Methods
        /// <summary>
        /// MarketData ClearData
        /// </summary>
        public void ClearData()
        {
            Assessments.Clear();
        }
        /// <summary>
        /// MarketData Register
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <param name="metadata">the entity of metadata</param>
        /// <returns></returns>
        public async Task Register(MarketDataEntity.Input metadata)
        {
            EnsureArg.IsNotNull(metadata, nameof(metadata));
            EnsureArg.IsTrue(metadata.ProviderName == null || metadata.ProviderName == this.Identifier.Provider);
            EnsureArg.IsTrue(metadata.ProviderName == null || metadata.ProviderName == this.Identifier.Provider);
            EnsureArg.IsTrue(metadata.MarketDataName == null || metadata.MarketDataName == this.Identifier.Name);
            EnsureArg.IsNotNullOrWhiteSpace(metadata.OriginalTimezone);

            metadata.ProviderName = this.Identifier.Provider;
            metadata.MarketDataName = this.Identifier.Name;

            if (_entity != null)
                throw new MarketAssessmentException("Actual Time Serie is already registered with ID {0}", _entity.MarketDataId);

            _entity = await _metadataService.RegisterMarketDataAsync(metadata);
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
                throw new MarketAssessmentException("Actual Time Serie is not yet registered");

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
        public async Task Update(MarketDataEntity.Input metadata)
        {
            if (_entity == null)
                throw new MarketAssessmentException("Actual Time Serie is not yet registered");

            _entity = await _metadataService.UpdateMarketDataAsync(metadata);
        }
        #endregion

        #region Write
        /// <summary>
        /// MarketAssessment AddData
        /// </summary>
        /// <remarks>
        /// Add Data on curve with localDate
        /// </remarks>
        /// <returns>AddAssessmentOperationResult</returns>
        public AddAssessmentOperationResult AddData(LocalDate localDate, string product, MarketAssessmentValue value)
        {
            Ensure.Bool.IsTrue(_isInWriteMode);

            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new MarketAssessmentException("This MarketData has Time granularity. Use AddData(Instant time...)");

            return _addAssessment(localDate.AtMidnight(), product, value);
        }
        /// <summary>
        /// MarketAssessment AddData
        /// </summary>
        /// <remarks>
        /// Add Data on curve with Instant
        /// </remarks>
        /// <returns>AddAssessmentOperationResult</returns>
        public AddAssessmentOperationResult AddData(Instant time, string product, MarketAssessmentValue value)
        {
            Ensure.Bool.IsTrue(_isInWriteMode);

            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new MarketAssessmentException("This MarketData has Date granularity. Use AddData(LocalDate date...)");

            return _addAssessment(time.InUtc().LocalDateTime, product, value);
        }

        private AddAssessmentOperationResult _addAssessment(LocalDateTime reportTime, string product, MarketAssessmentValue value)
        {
            if (product.Contains("-"))
            {
                if (_entity.OriginalGranularity.IsTimeGranularity())
                {
                    var period = ArtesianUtils.MapTimePeriod(_entity.OriginalGranularity);
                    if (!reportTime.IsStartOfInterval(period))
                        throw new MarketAssessmentException("Trying to insert Report Time {0} with wrong format to Assessment {1}. Should be of period {2}", reportTime, Identifier, period);
                }
                else
                {
                    var period = ArtesianUtils.MapDatePeriod(_entity.OriginalGranularity);
                    if (!reportTime.IsStartOfInterval(period))
                        throw new MarketAssessmentException("Trying to insert Report Time {0} with wrong format to Assessment {1}. Should be of period {2}", reportTime, Identifier, period);
                }

                //if (reportTime.Date >= product.ReferenceDate)
                //    return AddAssessmentOperationResult.IllegalReferenceDate;

                if (Assessments
                        .Any(row => row.ReportTime == reportTime && row.Product.Equals(product)))
                    return AddAssessmentOperationResult.ProductAlreadyPresent;

                Assessments.Add(new AssessmentElement(reportTime, product, value));
                return AddAssessmentOperationResult.AssessmentAdded;
            }
            else
            {

                if (Assessments.Any(row => row.ReportTime == reportTime && row.Product.Equals(product)))
                    return AddAssessmentOperationResult.ProductAlreadyPresent;

                Assessments.Add(new AssessmentElement(reportTime, product, value));
                return AddAssessmentOperationResult.AssessmentAdded;
            }

            throw new NotSupportedException("Invalid Product");
        }

        /// <summary>
        /// MarketAssessment Edit
        /// </summary>
        /// <remarks>
        /// Returns the MarketAssessment to start write operations
        /// </remarks>
        /// <returns>MarketAssessment</returns>
        public MarketAssessment EditActual()
        {
            Ensure.Any.IsNotNull(_entity);
            _isInWriteMode = true;

            return this;
        }
        /// <summary>
        /// MarketData Save
        /// </summary>
        /// <remarks>
        /// Save the Data of the current MarketData
        /// </remarks>
        /// <param name="downloadedAt">downloaded at</param>
        /// <param name="deferCommandExecution">deferCommandExecution</param>
        /// <param name="deferDataGeneration">deferDataGeneration</param>
        /// <returns></returns>
        public async Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true)
        {
            Ensure.Any.IsNotNull(_entity);
            Ensure.Bool.IsTrue(_isInWriteMode);

            if (Assessments.Any())
            {
                var data = new UpsertCurveData(this.Identifier);
                data.Timezone = DataTimezone;
                data.DownloadedAt = downloadedAt;
                data.DeferCommandExecution = deferCommandExecution;
                data.MarketAssessment = new Dictionary<LocalDateTime, IDictionary<string, MarketAssessmentValue>>();

                foreach (var reportTime in Assessments.GroupBy(g => g.ReportTime))
                {
                    var assessments = reportTime.ToDictionary(key => key.Product.ToString(), value => value.Value);
                    data.MarketAssessment.Add(reportTime.Key, assessments);
                }

                await _metadataService.UpsertCurveDataAsync(data);
            }
            //else
            //    _logger.Warn("No Data to be saved.");
        }

        #endregion

        /// <summary>
        /// AssessmentElement entity
        /// </summary>
        public class AssessmentElement
        {
            /// <summary>
            /// AssessmentElement constructor
            /// </summary>
            public AssessmentElement(LocalDateTime reportTime, string product, MarketAssessmentValue value)
            {
                ReportTime = reportTime;
                Product = product;
                Value = value;
            }

            /// <summary>
            /// AssessmentElement ReportTime
            /// </summary>
            public LocalDateTime ReportTime { get; set; }
            /// <summary>
            /// AssessmentElement Product
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// AssessmentElement MarketAssessmentValue
            /// </summary>
            public MarketAssessmentValue Value { get; set; }
        }
    }
}
