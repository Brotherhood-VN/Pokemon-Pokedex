using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IConditionService
    {
        Task<OperationResult> Create(ConditionDto dto);
        Task<OperationResult> Update(ConditionDto dto);
        Task<OperationResult> Delete(ConditionDto dto);
        Task<PaginationUtility<ConditionDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListCondition();
        Task<ConditionDto> GetDetail(long id);
    }
}