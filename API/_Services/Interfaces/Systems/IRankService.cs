using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IRankService
    {
        Task<OperationResult> Create(RankDto dto);
        Task<OperationResult> Update(RankDto dto);
        Task<OperationResult> Delete(RankDto dto);
        Task<PaginationUtility<RankDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListRank();
        Task<RankDto> GetDetail(long id);
    }
}