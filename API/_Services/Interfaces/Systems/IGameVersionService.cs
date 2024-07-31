using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IGameVersionService
    {
        Task<OperationResult> Create(GameVersionDto dto);
        Task<OperationResult> Update(GameVersionDto dto);
        Task<OperationResult> Delete(GameVersionDto dto);
        Task<PaginationUtility<GameVersionDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListGameVersion();
        Task<GameVersionDto> GetDetail(long id);
    }
}