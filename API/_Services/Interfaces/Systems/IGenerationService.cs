using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IGenerationService
    {
        Task<OperationResult> Create(GenerationDto dto);
        Task<OperationResult> Update(GenerationDto dto);
        Task<OperationResult> Delete(GenerationDto dto);
        Task<PaginationUtility<GenerationDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListGeneration();
        Task<GenerationDto> GetDetail(long id);
    }
}