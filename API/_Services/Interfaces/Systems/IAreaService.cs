using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IAreaService
    {
        Task<OperationResult> Create(AreaDto dto);
        Task<OperationResult> Update(AreaDto dto);
        Task<OperationResult> Delete(AreaDto dto);
        Task<PaginationUtility<AreaDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListArea();
        Task<AreaDto> GetDetail(long id);
    }
}