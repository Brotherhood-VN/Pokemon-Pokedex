using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IAbilityService
    {
        Task<OperationResult> Create(AbilityDto dto);
        Task<OperationResult> Update(AbilityDto dto);
        Task<OperationResult> Delete(AbilityDto dto);
        Task<PaginationUtility<AbilityDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListAbility();
        Task<AbilityDto> GetDetail(long id);
    }
}