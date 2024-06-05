using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IGenderService
    {
        Task<OperationResult> Create(GenderDto dto);
        Task<OperationResult> Update(GenderDto dto);
        Task<OperationResult> Delete(GenderDto dto);
        Task<PaginationUtility<GenderDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListGender();
        Task<GenderDto> GetDetail(long id);
    }
}