using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ISkillService
    {
        Task<OperationResult> Create(SkillDto dto);
        Task<OperationResult> Update(SkillDto dto);
        Task<OperationResult> Delete(SkillDto dto);
        Task<PaginationUtility<SkillDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListSkill();
        Task<SkillDto> GetDetail(long id);
    }
}