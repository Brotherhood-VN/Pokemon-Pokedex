using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IAccountTypeService
    {
        Task<OperationResult> Create(AccountTypeDto dto);
        Task<OperationResult> Update(AccountTypeDto dto);
        Task<OperationResult> Delete(AccountTypeDto dto);
        Task<PaginationUtility<AccountTypeDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListAccountType();
        Task<AccountTypeDto> GetDetail(long id);
    }
}