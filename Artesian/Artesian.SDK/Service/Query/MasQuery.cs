﻿// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using Flurl;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    public class MasQuery : Query, IMasQuery<MasQuery>
    {
        private IEnumerable<string> _products;
        private string _routePrefix = "mas";
        private Client _client;

        internal MasQuery(Client client)
        {
            _client = client;
        }

        #region facade methods
        /// <summary>
        /// Set of Market Data ID's to be queried
        /// </summary>
        /// <param name="ids">An Int array</param>
        /// <returns>MasQuery</returns>
        public MasQuery ForMarketData(int[] ids)
        {
            _ids = ids;
            return this;
        }
        /// <summary>
        /// Market Data ID to be queried
        /// </summary>
        /// <param name="id">An Int</param>
        /// <returns>MasQuery</returns>
        public MasQuery ForMarketData(int id)
        {
            _ids = new int[] { id };
            return this;
        }
        /// <summary>
        /// Timezone to be queried defaults to UTC
        /// </summary>
        /// <param name="tz">String</param>
        /// <returns>MasQuery</returns>
        public MasQuery InTimezone(string tz)
        {
            _inTimezone(tz);
            return this;
        }
        /// <summary>
        /// Date range to be queried
        /// </summary>
        /// <param name="start">LocalDate</param>
        /// <param name="end">LocalDate</param>
        /// <returns>MasQuery</returns>
        public MasQuery InAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            _inAbsoluteDateRange(start, end);
            return this;
        }
        /// <summary>
        /// Period Range to be queried
        /// </summary>
        /// <param name="from">Period</param>
        /// <param name="to">Period</param>
        /// <returns>MasQuery</returns>
        public MasQuery InRelativePeriodRange(Period from, Period to)
        {
            _inRelativePeriodRange(from, to);
            return this;
        }
        /// <summary>
        /// Period to be queried
        /// </summary>
        /// <param name="extractionPeriod">Period</param>
        /// <returns>MasQuery</returns>
        public MasQuery InRelativePeriod(Period extractionPeriod)
        {
            _inRelativePeriod(extractionPeriod);
            return this;
        }
        /// <summary>
        /// Interval to be queried
        /// </summary>
        /// <param name="relativeInterval">RelativeInterval</param>
        /// <returns>MasQuery</returns>
        public MasQuery InRelativeInterval(RelativeInterval relativeInterval)
        {
            _inRelativeInterval(relativeInterval);
            return this;
        }
        #endregion

        #region market assessment methods
        /// <summary>
        /// Products to be queried
        /// </summary>
        /// <param name="products">params string array</param>
        /// <returns></returns>
        public MasQuery ForProducts(params string[] products)
        {
            _products = products;
            return this;
        }
        /// <summary>
        /// Execute MasQuery
        /// </summary>
        /// <param name="ctk"></param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public async Task<IEnumerable<AssessmentRow>> ExecuteAsync(CancellationToken ctk = default)
        {
            return await _client.Exec<IEnumerable<AssessmentRow>>(HttpMethod.Get, _buildRequest(), ctk: ctk);
        }

        #region private
        string _buildRequest()
        {
            _validateQuery();

            var url = $"/{_routePrefix}/{_buildExtractionRangeRoute()}"
            .SetQueryParam("id", _ids)
            .SetQueryParam("p", _products)
            .SetQueryParam("tz", _tz);

            return url.ToString();
        }

        protected override void _validateQuery()
        {
            base._validateQuery();

            if (_products == null)
                throw new ApplicationException("Products must be provided for extraction");
        } 
        #endregion
        #endregion
    }
}