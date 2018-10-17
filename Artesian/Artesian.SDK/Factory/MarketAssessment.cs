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
    public class MarketAssessment : MarketData, IMarketAssessmentWritable
    {
        /// <summary>
        /// MarketData AssessmentElement
        /// </summary>
        public List<AssessmentElement> Assessments { get; protected set; }

        /// <summary>
        /// MarketAssessment Constructor
        /// </summary>
        public MarketAssessment(IMetadataService metadataService, MarketDataEntity.Output entity) : base (metadataService, entity)
        {
            Assessments = new List<AssessmentElement>();
        }

        /// <summary>
        /// MarketData ClearData
        /// </summary>
        public void ClearData()
        {
            Assessments.Clear();
        }

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

            if (Assessments.Any())
            {
                var data = new UpsertCurveData(this.Identifier);
                data.Timezone = _entity.OriginalTimezone;
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
