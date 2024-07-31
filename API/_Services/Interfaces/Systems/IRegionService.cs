using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IRegionService
    {
        Task<OperationResult> Create(RegionDto dto);
        Task<OperationResult> Update(RegionDto dto);
        Task<OperationResult> Delete(RegionDto dto);
        Task<PaginationUtility<RegionDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListRegion();
        Task<RegionDto> GetDetail(long id);
    }
}