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
    /// Versioned Time Serie Query Class
    /// </summary>
    public class VersionedQuery : Query, IVersionedQuery<VersionedQuery>
    {
        private VersionSelectionConfig _versionSelectionCfg = new VersionSelectionConfig();
        private VersionSelectionType? _versionSelectionType = null;
        private Granularity? _granularity;
        private Client _client;
        private IPartitionStrategy _partition;

        private int? _tr;
        private string _routePrefix = "vts";

        internal VersionedQuery(Client client, IPartitionStrategy partiton)
        {
            _client = client;
            _partition = partiton;
        }

        #region facade methods
        /// <summary>
        /// Set the list of marketdata to be queried
        /// </summary>
        /// <param name="ids">Array of marketdata id's to be queried</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery ForMarketData(int[] ids)
        {
            _forMarketData(ids);
            return this;
        }
        /// <summary>
        /// Set the marketdata to be queried
        /// </summary>
        /// <param name="id">The marketdata id to be queried</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery ForMarketData(int id)
        {
            _forMarketData(new int[] { id });
            return this;
        }
        /// <summary>
        /// Set the filter id to be queried
        /// </summary>
        /// <param name="filterId">The filter id to be queried</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery ForFilterId(int filterId)
        {
            _forFilterId(filterId);
            return this;
        }
        /// <summary>
        /// Specify the timezone of extracted marketdata. Defaults to UTC
        /// </summary>
        /// <param name="tz">Timezone in which to extract eg UTC/CET</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery InTimezone(string tz)
        {
            _inTimezone(tz);
            return this;
        }
        /// <summary>
        /// Set the date range to be queried
        /// </summary>
        /// <param name="start">Start date of range</param>
        /// <param name="end">End date of range</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery InAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            _inAbsoluteDateRange(start, end);
            return this;
        }
        /// <summary>
        /// Set the relative period range from today to be queried
        /// </summary>
        /// <param name="from">Start period of range</param>
        /// <param name="to">End period of range</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery InRelativePeriodRange(Period from, Period to)
        {
            _inRelativePeriodRange(from, to);
            return this;
        }
        /// <summary>
        /// Set the relative period from today to be queried
        /// </summary>
        /// <param name="extractionPeriod">Period to be queried</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery InRelativePeriod(Period extractionPeriod)
        {
            _inRelativePeriod(extractionPeriod);
            return this;
        }
        /// <summary>
        /// Set the relative interval to be queried
        /// </summary>
        /// <param name="relativeInterval">The relative interval to be queried</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery InRelativeInterval(RelativeInterval relativeInterval)
        {
            _inRelativeInterval(relativeInterval);
            return this;
        }
        /// <summary>
        /// Set the time transform to be applied to the extraction
        /// </summary>
        /// <param name="tr">The Time Tramsform id to be applied to the extraction</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery WithTimeTransform(int tr)
        {
            _tr = tr;
            return this;
        }
        /// <summary>
        /// Set the time transform to be applied to the extraction
        /// </summary>
        /// <param name="tr">The system defined time transform to be applied to the extraction</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery WithTimeTransform(SystemTimeTransform tr)
        {
            _tr = (int)tr;
            return this;
        }
        #endregion

        #region versioned query methods
        /// <summary>
        /// Set the granularity of the extracted marketdata
        /// </summary>
        /// <param name="granularity">The granulairty in which to extract data. See <see cref="Granularity"/> for types of Granularity</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery InGranularity(Granularity granularity)
        {
            _granularity = granularity;
            return this;
        }
        /// <summary>
        /// Set the number of versions to retrieve in the extraction
        /// </summary>
        /// <param name="lastN">The number of previous versions to extract</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery ForLastNVersions(int lastN)
        {
            _versionSelectionType = VersionSelectionType.LastN;
            _versionSelectionCfg.LastN = lastN;
            return this;
        }
        /// <summary>
        /// Set the version selection type to MUV. 
        /// </summary>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery ForMUV()
        {
            _versionSelectionType = VersionSelectionType.MUV;
            return this;
        }
        /// <summary>
        /// Set Last Of Days date range version selection
        /// </summary>
        /// <param name="start">Start date of version range</param>
        /// <param name="end">End date of version range</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery ForLastOfDays(LocalDate start, LocalDate end)
        {
            if (end <= start)
                throw new ArgumentException("End date " + end + " must be greater than start date " + start);

            _versionSelectionType = VersionSelectionType.LastOfDays;
            _versionSelectionCfg.LastOf.DateStart = start;
            _versionSelectionCfg.LastOf.DateEnd = end;

            return this;
        }
        /// <summary>
        /// Set Last Of Days period range version selection
        /// </summary>
        /// <param name="from">Start period of version range</param>
        /// <param name="to">End period of version range</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery ForLastOfDays(Period from, Period to)
        {
            _versionSelectionType = VersionSelectionType.LastOfDays;
            _versionSelectionCfg.LastOf.PeriodFrom = from;
            _versionSelectionCfg.LastOf.PeriodTo = to;

            return this;
        }
        /// <summary>
        /// Set Last Of Days period version selection
        /// </summary>
        /// <param name="lastOfPeriod">Period of version range</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery ForLastOfDays(Period lastOfPeriod)
        {
            _versionSelectionType = VersionSelectionType.LastOfDays;
            _versionSelectionCfg.LastOf.Period = lastOfPeriod;

            return this;
        }
        /// <summary>
        /// Set Last of Months date range version selection
        /// </summary>
        /// <param name="start">Start date of version range</param>
        /// <param name="end">End date of version range</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery ForLastOfMonths(LocalDate start, LocalDate end)
        {
            if (end <= start)
                throw new ArgumentException("End date " + end + " must be greater than start date " + start);

            _versionSelectionType = VersionSelectionType.LastOfMonths;
            _versionSelectionCfg.LastOf.DateStart = start;
            _versionSelectionCfg.LastOf.DateEnd = end;

            return this;
        }
        /// <summary>
        /// Set Last of Months period version selection
        /// </summary>
        /// <param name="lastOfPeriod">Period of version range</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery ForLastOfMonths(Period lastOfPeriod)
        {
            _versionSelectionType = VersionSelectionType.LastOfMonths;
            _versionSelectionCfg.LastOf.Period = lastOfPeriod;

            return this;
        }
        /// <summary>
        /// Set Last of Months period range version selection
        /// </summary>
        /// <param name="from">Start period of version range</param>
        /// <param name="to">End period of version range</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery ForLastOfMonths(Period from, Period to)
        {
            _versionSelectionType = VersionSelectionType.LastOfMonths;
            _versionSelectionCfg.LastOf.PeriodFrom = from;
            _versionSelectionCfg.LastOf.PeriodTo = to;

            return this;
        }
        /// <summary>
        /// Set specific version selection
        /// </summary>
        /// <param name="version">Date time of the version to be extracted</param>
        /// <returns>VersionedQuery</returns>
        public VersionedQuery ForVersion(LocalDateTime version)
        {
            _versionSelectionType = VersionSelectionType.Version;
            _versionSelectionCfg.Version = version;

            return this;
        }
        /// <summary>
        /// Execute VersionedQuery
        /// </summary>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Enumerable of TimeSerieRow Versioned</returns>
        public async Task<IEnumerable<TimeSerieRow.Versioned>> ExecuteAsync(CancellationToken ctk = default)
        {
            List<string> urls = _buildRequest();

            var taskList = urls.Select(url => _client.Exec<IEnumerable<TimeSerieRow.Versioned>>(HttpMethod.Get, url, ctk: ctk));

            var res = await Task.WhenAll(taskList);

            return res.SelectMany(t=>t);
        }

        #region private
        /// <summary>
        /// Validate Query override
        /// </summary>
        protected override void _validateQuery()
        {
            base._validateQuery();

            if (_granularity == null)
                throw new ApplicationException("Extraction granularity must be provided. Use .InGranularity() argument takes a granularity type");

            if (_versionSelectionType == null)
                throw new ApplicationException("Version selection must be provided. Provide a version to query. eg .ForLastOfDays() arguments take a date range , period or period range");
        }

        private string _buildVersionRoute()
        {
            string subPath;

            switch (_versionSelectionType.Value)
            {
                case VersionSelectionType.LastN:
                    subPath = $"Last{_versionSelectionCfg.LastN}";
                    break;
                case VersionSelectionType.MUV:
                    subPath = $"MUV";
                    break;
                case VersionSelectionType.LastOfDays:
                case VersionSelectionType.LastOfMonths:
                    subPath = _buildLastOfSubRoute();
                    break;
                case VersionSelectionType.Version:
                    subPath = $"Version/{_toUrlParam(_versionSelectionCfg.Version)}";
                    break;
                default:
                    throw new Exception("Unsupported version type");
            }

            return subPath;
        }

        private string _buildLastOfSubRoute()
        {
            string subPath;

            if (_versionSelectionCfg.LastOf.DateStart != null && _versionSelectionCfg.LastOf.DateEnd !=null)
                subPath = $"{_versionSelectionType}/{_toUrlParam(_versionSelectionCfg.LastOf.DateStart.Value,_versionSelectionCfg.LastOf.DateEnd.Value)}";
            else if (_versionSelectionCfg.LastOf.Period != null)
                subPath = $"{_versionSelectionType}/{_versionSelectionCfg.LastOf.Period}";
            else if (_versionSelectionCfg.LastOf.PeriodFrom != null && _versionSelectionCfg.LastOf.PeriodTo != null)
                subPath = $"{_versionSelectionType}/{_versionSelectionCfg.LastOf.PeriodFrom}/{_versionSelectionCfg.LastOf.PeriodTo}";
            else
                throw new ApplicationException("LastOf extraction type not defined");

            return subPath;
        }

        private List<string> _buildRequest()
        {
            _validateQuery();

            string url = null;
            List<string> urlList = new List<string>();

            var versionedParams = new VersionedQueryParamaters(_ids, _extractionRangeCfg, _extractionRangeType, _versionSelectionCfg, _versionSelectionType, _granularity, _tr);

            if (_ids != null)
            {
                urlList = _partition.Partition(new List<VersionedQueryParamaters>() { versionedParams })
                    .Select(qp => $"/{_routePrefix}/{_buildVersionRoute()}/{_granularity}/{_buildExtractionRangeRoute()}"
                            .SetQueryParam("id", qp.Ids)
                            .SetQueryParam("tz", _tz)
                            .SetQueryParam("tr", qp.Tr)
                            .ToString())
                    .ToList();
            }
            else
            {
                url = $"/{_routePrefix}/{_buildVersionRoute()}/{_granularity}/{_buildExtractionRangeRoute()}"
                            .SetQueryParam("filterId", _filterId)
                            .SetQueryParam("tz", _tz)
                            .SetQueryParam("tr", _tr);

                urlList.Add(url);
            }

            return urlList;
        }
        #endregion

        #endregion

    }
}