using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IStatTypeService
    {
        Task<OperationResult> Create(StatTypeDto dto);
        Task<OperationResult> Update(StatTypeDto dto);
        Task<OperationResult> Delete(StatTypeDto dto);
        Task<PaginationUtility<StatTypeDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListStatType();
        Task<StatTypeDto> GetDetail(long id);
    }
}