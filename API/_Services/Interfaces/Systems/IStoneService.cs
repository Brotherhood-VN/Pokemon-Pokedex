using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IStoneService
    {
        Task<OperationResult> Create(StoneDto dto);
        Task<OperationResult> Update(StoneDto dto);
        Task<OperationResult> Delete(StoneDto dto);
        Task<PaginationUtility<StoneDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListStone();
        Task<StoneDto> GetDetail(long id);
    }
}