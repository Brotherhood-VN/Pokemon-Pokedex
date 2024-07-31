using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IItemService
    {
        Task<OperationResult> Create(ItemDto dto);
        Task<OperationResult> Update(ItemDto dto);
        Task<OperationResult> Delete(ItemDto dto);
        Task<PaginationUtility<ItemDto>> GetDataPagination(PaginationParam pagination, string keyword);
        Task<List<KeyValuePair<long, string>>> GetListItem();
        Task<ItemDto> GetDetail(long id);
    }
}