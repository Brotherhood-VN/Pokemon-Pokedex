using API.Dtos.Systems;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface ITranslationService
    {
        Task<OperationResult> Create(TranslationDto dto);
        Task<OperationResult> Update(TranslationDto dto);
        Task<OperationResult> Delete(TranslationDto dto);
        Task<PaginationUtility<TranslationDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListTranslation();
        Task<TranslationDto> GetDetail(long id);
    }
}