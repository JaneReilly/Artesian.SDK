using Artesian.SDK.Dto;
using Artesian.SDK.Dto.PublicOffer;
using Flurl;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service.PublicOffer
{
    /// <summary>
    /// PublicOfferService class
    /// Contains query types to be created
    /// </summary>
    public partial class PublicOfferService
    {

        /// <summary>
        /// Get paged OperatorDto 
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="operatorFilter">Operator substring filter</param>
        /// <param name="sort">Sort by</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of OperatorDto</returns>
        public Task<PagedResult<OperatorDto>> ReadOperatorsAsync(int page, int pageSize, string operatorFilter = null, string[] sort = null, CancellationToken ctk = default)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and Page number need to be greater than 0. Page:" + page + " Page Size:" + pageSize);

            var url = "/enums/operators"
                .SetQueryParam("operatorFilter", operatorFilter)
                .SetQueryParam("sort", sort)
                .SetQueryParam("pageSize", pageSize)
                .SetQueryParam("page", page)
                ;

            return _client.Exec<PagedResult<OperatorDto>>(HttpMethod.Get, url);
        }

        /// <summary>
        /// Get paged UnitDto 
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="unitFilter">Unit substring filter</param>
        /// <param name="sort">Sort by</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of UnitDto</returns>
        public Task<PagedResult<UnitDto>> ReadUnitsAsync(int page, int pageSize, string unitFilter = null, string[] sort = null, CancellationToken ctk = default)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and Page number need to be greater than 0. Page:" + page + " Page Size:" + pageSize);

            var url = "/enums/units"
                .SetQueryParam("unitFilter", unitFilter)
                .SetQueryParam("sort", sort)
                .SetQueryParam("pageSize", pageSize)
                .SetQueryParam("page", page)
                ;

            return _client.Exec<PagedResult<UnitDto>>(HttpMethod.Get, url);
        }


    }
}
