using Artesian.SDK.Dto;
using Artesian.SDK.Dto.PublicOffer;
using Flurl;
using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service.PublicOffer
{
    /// <summary>
    /// Public Offer Query Class
    /// </summary>
    public class PublicOfferQuery: IPublicOfferQuery
    {
        private Client _client;
        private PublicOfferQueryParamaters _queryParamaters = new PublicOfferQueryParamaters();
        private static LocalDatePattern _localDatePattern = LocalDatePattern.Iso;

        internal PublicOfferQuery(Client client)
        {
            _client = client;
        }

        /// <summary>
        /// Set the date to be queried
        /// </summary>
        /// <param name="date">Date to be queried</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery ForDate(LocalDate date)
        {
            _queryParamaters.Date = date;

            return this;
        }

        /// <summary>
        /// Set the Status to be queried
        /// </summary>
        /// <param name="status">Status to be queried</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery ForStatus(Status status)
        {
            _queryParamaters.Status = status;

            return this;
        }

        /// <summary>
        /// Set the Purpose to be queried
        /// </summary>
        /// <param name="purpose">Purpose to be queried</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery ForPurpose(Purpose purpose)
        {
            _queryParamaters.Purpose = purpose;

            return this;
        }

        /// <summary>
        /// Set the BATypes to be queried
        /// </summary>
        /// <param name="baType">BATypes to be queried</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery ForBAType(BAType[] baType)
        {
            _queryParamaters.BAType = baType;

            return this;
        }

        /// <summary>
        /// Set the generation types to be queried
        /// </summary>
        /// <param name="generationType">GenerationTypes to be queried</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery ForGenerationType(GenerationType[] generationType)
        {
            _queryParamaters.GenerationType = generationType;

            return this;
        }

        /// <summary>
        /// Set the markets to be queried
        /// </summary>
        /// <param name="market">Markets to be queried</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery ForMarket(Market[] market)
        {
            _queryParamaters.Market = market;

            return this;
        }

        /// <summary>
        /// Set the operators to be queried
        /// </summary>
        /// <param name="operator">Operators to be queried</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery ForOperator(string[] @operator)
        {
            _queryParamaters.Operator = @operator;

            return this;
        }

        /// <summary>
        /// Set the scopes to be queried
        /// </summary>
        /// <param name="scope">Scopes to be queried</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery ForScope(Scope[] scope)
        {
            _queryParamaters.Scope = scope;

            return this;
        }

        /// <summary>
        /// Set the units to be queried
        /// </summary>
        /// <param name="unit">Units to be queried</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery ForUnit(string[] unit)
        {
            _queryParamaters.Unit = unit;

            return this;
        }

        /// <summary>
        /// Set the unot types to be queried
        /// </summary>
        /// <param name="unitType">Unit types to be queried</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery ForUnitType(UnitType[] unitType)
        {
            _queryParamaters.UnitType = unitType;

            return this;
        }

        /// <summary>
        /// Set the zones to be queried
        /// </summary>
        /// <param name="zone">Zones to be queried</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery ForZone(Zone[] zone)
        {
            _queryParamaters.Zone = zone;

            return this;
        }

        /// <summary>
        /// Set the request pagination
        /// </summary>
        /// <param name="page">Result page number</param>
        /// <param name="pageSize">Result Page size</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery WithPagination(int page, int pageSize)
        {
            _queryParamaters.Page = page;
            _queryParamaters.PageSize = pageSize;

            return this;
        }

        /// <summary>
        /// Set the data sort
        /// </summary>
        /// <param name="sort">Sort by</param>
        /// <returns>PublicOfferQuery</returns>
        public PublicOfferQuery WithSort(string[] sort)
        {
            _queryParamaters.Sort = sort;

            return this;
        }

        /// <summary>
        /// Execute Public Offer Query
        /// </summary>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Enumerable of TimeSerieRow Actual</returns>
        public async Task<PagedResult<PublicOfferCurve>> ExecuteAsync(CancellationToken ctk = default)
        {
            string url = _buildRequest();

            var res = await _client.Exec<PagedResult<PublicOfferCurve>>(HttpMethod.Get, url, ctk: ctk);
            return res;
        }


        #region private helpers
        private string _buildRequest()
        {
            _validateQuery();

            var url = $"/extract/{_localDatePattern.Format(_queryParamaters.Date.Value)}/{_queryParamaters.Purpose.ToString()}/{_queryParamaters.Status.ToString()}"
                        .SetQueryParam("operators", _queryParamaters.Operator)
                        .SetQueryParam("unit", _queryParamaters.Unit)
                        .SetQueryParam("market", _queryParamaters.Market)
                        .SetQueryParam("scope", _queryParamaters.Scope)
                        .SetQueryParam("baType", _queryParamaters.BAType)
                        .SetQueryParam("zone", _queryParamaters.Zone)
                        .SetQueryParam("unitType", _queryParamaters.UnitType)
                        .SetQueryParam("generationType", _queryParamaters.GenerationType)
                        .SetQueryParam("sort", _queryParamaters.Sort)
                        .SetQueryParam("page", _queryParamaters.Page)
                        .SetQueryParam("pageSize", _queryParamaters.PageSize)
                        .ToString();

            return url;
        }

        /// <summary>
        /// Validate query
        /// </summary>
        protected void _validateQuery()
        {
            if (_queryParamaters.Date == null)
                throw new ApplicationException("Date filter must be provided. Use .ForDate() to set date");
            
            if (_queryParamaters.Status == null)
                throw new ApplicationException("Status filter must be provided. Use .ForStatus() to set status");
            
            if (_queryParamaters.Purpose == null)
                throw new ApplicationException("Purpose filter must be provided. Use .ForPurpose() to set purpose");
        }
        #endregion
    }
}
