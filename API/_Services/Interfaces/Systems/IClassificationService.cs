using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IClassificationService
    {
        Task<OperationResult> Create(ClassificationDto dto);
        Task<OperationResult> Update(ClassificationDto dto);
        Task<OperationResult> Delete(ClassificationDto dto);
        Task<PaginationUtility<ClassificationDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListClassification();
        Task<ClassificationDto> GetDetail(long id);
    }
}