// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using Flurl;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Market Assessment Query Class
    /// </summary>
    public class MasQuery : Query<MasQueryParamaters>, IMasQuery<MasQuery>
    {        
        private string _routePrefix = "mas";
        private Client _client;
        private IPartitionStrategy _partition;

        internal MasQuery(Client client, IPartitionStrategy partiton)
        {
            _client = client;
            _partition = partiton;
        }

        #region facade methods
        /// <summary>
        /// Set the list of marketdata to be queried
        /// </summary>
        /// <param name="ids">Array of marketdata id's to be queried</param>
        /// <returns>MasQuery</returns>
        public MasQuery ForMarketData(int[] ids)
        {
            _forMarketData(ids);
            return this;
        }
        /// <summary>
        /// Set the marketdata to be queried
        /// </summary>
        /// <param name="id">The marketdata id to be queried</param>
        /// <returns>MasQuery</returns>
        public MasQuery ForMarketData(int id)
        {
            _forMarketData(new int[] { id });
            return this;
        }
        /// <summary>
        /// Set the filter id to be queried
        /// </summary>
        /// <param name="filterId">The filter id to be queried</param>
        /// <returns>MasQuery</returns>
        public MasQuery ForFilterId(int filterId)
        {
            _forFilterId(filterId);
            return this;
        }
        /// <summary>
        /// Specify the timezone of extracted marketdata. Defaults to UTC
        /// </summary>
        /// <param name="tz">Timezone in which to extract eg UTC/CET</param>
        /// <returns>MasQuery</returns>
        public MasQuery InTimezone(string tz)
        {
            _inTimezone(tz);
            return this;
        }
        /// <summary>
        /// Set the date range to be queried
        /// </summary>
        /// <param name="start">Start date of range</param>
        /// <param name="end">End date of range</param>
        /// <returns>MasQuery</returns>
        public MasQuery InAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            _inAbsoluteDateRange(start, end);
            return this;
        }
        /// <summary>
        /// Set the relative period range from today to be queried
        /// </summary>
        /// <param name="from">Start period of range</param>
        /// <param name="to">End period of range</param>
        /// <returns>MasQuery</returns>
        public MasQuery InRelativePeriodRange(Period from, Period to)
        {
            _inRelativePeriodRange(from, to);
            return this;
        }
        /// <summary>
        /// Set the relative period from today to be queried
        /// </summary>
        /// <param name="extractionPeriod">Period to be queried</param>
        /// <returns>MasQuery</returns>
        public MasQuery InRelativePeriod(Period extractionPeriod)
        {
            _inRelativePeriod(extractionPeriod);
            return this;
        }
        /// <summary>
        /// Set the relative interval to be queried
        /// </summary>
        /// <param name="relativeInterval">The relative interval to be queried</param>
        /// <returns>MasQuery</returns>
        public MasQuery InRelativeInterval(RelativeInterval relativeInterval)
        {
            _inRelativeInterval(relativeInterval);
            return this;
        }
        #endregion

        #region market assessment methods
        /// <summary>
        /// Set list of market products to be queried
        /// </summary>
        /// <param name="products">List of products to be queried</param>
        /// <returns>MasQuery</returns>
        public MasQuery ForProducts(params string[] products)
        {
            _queryParamaters.Products = products;
            return this;
        }
        /// <summary>
        /// Set the Filler strategy to Null
        /// </summary>
        /// <returns>MasQuery</returns>
        public MasQuery WithFillNull()
        {
            _queryParamaters.FillerKind = FillerKind.Null;
            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Custom Value
        /// </summary>
        /// <param name="settlement"></param>
        /// <param name="open"></param>
        /// <param name="close"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="volumePaid"></param>
        /// <param name="volumeGiven"></param>
        /// <param name="volumeTotal"></param>
        /// <returns>MasQuery</returns>
        public MasQuery WithFillCustom(double settlement, double open, double close, double high, double low, double volumePaid, double volumeGiven, double volumeTotal)
        {
            _queryParamaters.FillerKind = FillerKind.CustomValue;
            _queryParamaters.FillerDVs = settlement;
            _queryParamaters.FillerDVo = open;
            _queryParamaters.FillerDVc = close;
            _queryParamaters.FillerDVh = high;
            _queryParamaters.FillerDVl = low;
            _queryParamaters.FillerDVvp = volumePaid;
            _queryParamaters.FillerDVvg = volumeGiven;
            _queryParamaters.FillerDVvt = volumeTotal;

            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Latest Value
        /// </summary>
        /// <param name="period"></param>
        /// <returns>MasQuery</returns>
        public MasQuery WithFillLatestValue(Period period)
        {
            _queryParamaters.FillerKind = FillerKind.LatestValidValue;
            _queryParamaters.FillerPeriod = period;

            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Fill None
        /// </summary>
        /// <returns>MasQuery</returns>
        public MasQuery WithFillNone()
        {
            _queryParamaters.FillerKind = FillerKind.NoFill;

            return this;
        }
        /// <summary>
        /// Execute MasQuery
        /// </summary>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Enumerable of AssessmentRow</returns>
        public async Task<IEnumerable<AssessmentRow>> ExecuteAsync(CancellationToken ctk = default)
        {
            List<string> urls = _buildRequest();

            var taskList = urls.Select(url => _client.Exec<IEnumerable<AssessmentRow>>(HttpMethod.Get, url, ctk: ctk));

            var res = await Task.WhenAll(taskList);

            return res.SelectMany(x => x);
        }

        #region private
        private List<string> _buildRequest()
        {
            _validateQuery();


            var urlList = _partition.Partition(new List<MasQueryParamaters> { _queryParamaters })
                .Select(qp => $"/{_routePrefix}/{_buildExtractionRangeRoute(qp)}"
                        .SetQueryParam("id", qp.Ids)
                        .SetQueryParam("filterId", qp.FilterId)
                        .SetQueryParam("p", qp.Products)
                        .SetQueryParam("tz", qp.TimeZone)
                        .SetQueryParam("fillerK",qp.FillerKind)
                        .SetQueryParam("fillerDVs",qp.FillerDVs)
                        .SetQueryParam("fillerDVo", qp.FillerDVo)
                        .SetQueryParam("fillerDVc", qp.FillerDVc)
                        .SetQueryParam("fillerDVh", qp.FillerDVh)
                        .SetQueryParam("fillerDVl", qp.FillerDVl)
                        .SetQueryParam("fillerDVvp", qp.FillerDVvp)
                        .SetQueryParam("fillerDVvg", qp.FillerDVvg)
                        .SetQueryParam("fillerDVvt", qp.FillerDVvt)
                        .SetQueryParam("fillerP" ,qp.FillerPeriod)
                        .ToString())
                .ToList();

            return urlList;
        }
        /// <summary>
        /// Validate Query override
        /// </summary>
        protected override void _validateQuery()
        {
            base._validateQuery();

            if (_queryParamaters.Products == null)
                throw new ApplicationException("Products must be provided for extraction. Use .ForProducts() argument takes a string or string array of products");

            if(_queryParamaters.FillerKind == FillerKind.Default)
            {
                WithFillLatestValue(Period.FromDays(14));
            }

            if (_queryParamaters.FillerKind == FillerKind.LatestValidValue)
            {
                if (_queryParamaters.FillerPeriod.ToString().Contains('-') == true || _queryParamaters.FillerPeriod == null)
                {
                    throw new ApplicationException("Latest valid value filler must contain a non negative Period");
                }
            }

            if (_queryParamaters.FillerKind == FillerKind.CustomValue)
            {
                if (_queryParamaters.FillerDVs == null || _queryParamaters.FillerDVo == null || _queryParamaters.FillerDVc == null || _queryParamaters.FillerDVh == null || _queryParamaters.FillerDVl == null || _queryParamaters.FillerDVvp == null || _queryParamaters.FillerDVvg == null || _queryParamaters.FillerDVvt == null)
                {
                    throw new ApplicationException("All 8 Filler default values must be provided. Provide values for all default values when using custom value filler");
                }
            }
        }
        #endregion
        #endregion
    }
}