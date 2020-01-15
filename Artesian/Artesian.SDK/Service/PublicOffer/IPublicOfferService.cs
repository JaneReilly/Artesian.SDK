using Artesian.SDK.Dto;
using Artesian.SDK.Dto.PublicOffer;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service.PublicOffer
{
    /// <summary>
    /// Public Offer Service Interface
    /// </summary>
    public interface IPublicOfferService
    {
        /// <summary>
        /// Create Public Offer Query
        /// </summary>
        /// <returns></returns>
        PublicOfferQuery CreateRawCurveQuery();

        /// <summary>
        /// Get paged OperatorDto 
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="operatorFilter">Operator substring filter</param>
        /// <param name="sort">Sort by</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of OperatorDto</returns>
        Task<PagedResult<OperatorDto>> ReadOperatorsAsync(int page, int pageSize, string operatorFilter = null, string[] sort = null, CancellationToken ctk = default);

        /// <summary>
        /// Get paged UnitDto 
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="unitFilter">Unit substring filter</param>
        /// <param name="sort">Sort by</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of UnitDto</returns>
        Task<PagedResult<UnitDto>> ReadUnitsAsync(int page, int pageSize, string unitFilter = null, string[] sort = null, CancellationToken ctk = default);
    }
}
