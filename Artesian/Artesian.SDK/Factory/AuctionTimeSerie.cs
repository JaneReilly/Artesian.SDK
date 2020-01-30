using Artesian.SDK.Dto;
using Artesian.SDK.Service;
using EnsureThat;
using NodaTime;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// AuctionTimeSerie entity
    /// </summary>
    internal sealed class AuctionTimeSerie : IAuctionMarketDataWritable
    {
        private IMarketDataService _marketDataService;
        private MarketDataEntity.Output _entity = null;
        private readonly MarketDataIdentifier _identifier = null;
        private Dictionary<LocalDateTime, AuctionBids> _bids = new Dictionary<LocalDateTime, AuctionBids>();

        /// <summary>
        /// AuctionTimeSerie Constructor
        /// </summary>
        internal AuctionTimeSerie(MarketData marketData)
        {
            _entity = marketData._entity;
            _marketDataService = marketData._marketDataService;

            _identifier = new MarketDataIdentifier(_entity.ProviderName, _entity.MarketDataName);

            AuctionBids = new ReadOnlyDictionary<LocalDateTime, AuctionBids>(_bids);
        }

        /// <summary>
        /// AuctionData Bid
        /// </summary>
        public IReadOnlyDictionary<LocalDateTime, AuctionBids> AuctionBids { get; private set; }

        /// <summary>
        /// MarketData ClearData
        /// </summary>
        public void ClearData()
        {
            _bids = new Dictionary<LocalDateTime, AuctionBids>();
            AuctionBids = new ReadOnlyDictionary<LocalDateTime, AuctionBids>(_bids);
        }

        /// <summary>
        /// AuctionTimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with localDate
        /// </remarks>
        /// <returns>AddTimeSerieOperationResult</returns>
        public AddTimeSerieOperationResult AddData(LocalDate localDate, AuctionBidValue[] bid, AuctionBidValue[] offer)
        {
            Ensure.Any.IsNotNull(_entity);

            var localTime = localDate.AtMidnight();

            return _add(localTime, bid, offer);
        }

        /// <summary>
        /// AuctionTimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with Instant
        /// </remarks>
        /// <returns>AddTimeSerieOperationResult</returns>
        public AddTimeSerieOperationResult AddData(Instant time, AuctionBidValue[] bid, AuctionBidValue[] offer)
        {
            Ensure.Any.IsNotNull(_entity);

            var localTime = time.InUtc().LocalDateTime;

            return _add(localTime, bid, offer);
        }

        private AddTimeSerieOperationResult _add(LocalDateTime bidTime, AuctionBidValue[] bid, AuctionBidValue[] offer)
        {
            _bids.Add(bidTime, new AuctionBids(bidTime, bid, offer));
            return AddTimeSerieOperationResult.ValueAdded;
        }

        /// <summary>
        /// MarketData Save
        /// </summary>
        /// <remarks>
        /// Save the Data of the current MarketData
        /// </remarks>
        /// <param name="downloadedAt">Downloaded at</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <returns></returns>
        public async Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true)
        {
            Ensure.Any.IsNotNull(_entity);

            if (AuctionBids.Any())
            {
                var data = new UpsertCurveData(_identifier)
                {
                    Timezone = _entity.OriginalGranularity.IsTimeGranularity() ? "UTC" : _entity.OriginalTimezone,
                    DownloadedAt = downloadedAt,
                    AuctionRows = _bids,
                    DeferCommandExecution = deferCommandExecution,
                    DeferDataGeneration = deferDataGeneration
                };

                await _marketDataService.UpsertCurveDataAsync(data);
            }
            //else
            //    _logger.Warn("No Data to be saved.");
        }
    }
}