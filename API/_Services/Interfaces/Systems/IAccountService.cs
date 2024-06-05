using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IAccountService
    {
        Task<OperationResult> Create(AccountDto dto);
        Task<OperationResult> ChangePassword(AccountChangePassword dto);
        Task<OperationResult> ResetPassword(long id);
        Task<OperationResult> Update(AccountDto dto);
        Task<OperationResult> Delete(AccountDto dto);
        Task<PaginationUtility<AccountDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<AccountDto> GetDetail(long id);
    }
}