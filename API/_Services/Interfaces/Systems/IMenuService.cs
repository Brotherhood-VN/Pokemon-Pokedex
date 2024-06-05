using API.Dtos.Systems;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Systems
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IMenuService
    {
        Task<List<TreeNode<MenuDto>>> LoadMenus();
        Task<OperationResult> Create(MenuDto dto);
        Task<OperationResult> Update(MenuDto dto);
        Task<OperationResult> Delete(long id);
        Task<OperationResult> ConfigurationMenus(List<TreeNode<MenuDto>> nodes);
        Task<List<KeyValuePair<string, string>>> GetListController();
    }
}