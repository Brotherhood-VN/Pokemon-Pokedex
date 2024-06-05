using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IRoleService
    {
        Task<OperationResult> Create(RoleDto dto);
        Task<OperationResult> Update(RoleDto dto);
        Task<OperationResult> Delete(RoleDto dto);
        Task<PaginationUtility<RoleDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListRole();
        Task<RoleDto> GetDetail(long id);
    }
}