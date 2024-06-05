using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IFunctionService
    {
        Task<OperationResult> Create(FunctionDto dto);
        Task<OperationResult> CreateRange(FunctionViewDto dto);

        Task<OperationResult> Update(FunctionDto dto);
        Task<OperationResult> UpdateRange(FunctionViewDto dto);

        Task<OperationResult> Delete(FunctionDto dto);
        Task<OperationResult> DeleteRange(FunctionViewDto dto);

        Task<OperationResult> CreateAllFunction(long userId, string controller);

        Task<PaginationUtility<FunctionViewDto>> GetDataPagination(PaginationParam pagination, FunctionSearchParam param, bool isPaging = true);
        Task<FunctionViewDto> GetDetailGroup(FunctionParam param);
        Task<FunctionDto> GetDetail(FunctionParam param);

        Task<List<KeyValuePair<string, string>>> GetListArea();
        Task<List<KeyValuePair<string, string>>> GetListController(string area);
        Task<List<KeyValuePair<string, string>>> GetListAction(string area, string controller);
    }
}