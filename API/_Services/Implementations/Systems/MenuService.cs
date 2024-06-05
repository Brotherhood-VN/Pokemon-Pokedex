using API._Services.Interfaces.Systems;
using API.Data;
using API.Dtos.Systems;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Implementations.Systems
{
    public class MenuService : IMenuService
    {
        private readonly DBContext _context;
        public MenuService(DBContext context)
        {
            _context = context;
        }

        #region LoadMenus
        public async Task<List<TreeNode<MenuDto>>> LoadMenus()
        {
            var menus = await _context.Menu.AsNoTracking()
                .Select(x => new TreeNode<MenuDto>
                {
                    Key = x.Id.ToString(),
                    Data = new MenuDto
                    {
                        Id = x.Id,
                        Controller = x.Controller,
                        Badge = x.Badge,
                        Class = x.Class,
                        Label = x.Label,
                        RouterLink = x.RouterLink,
                        Separator = x.Separator,
                        Target = x.Target,
                        Visible = x.Visible,
                        Icon = x.Icon,
                        Title = x.Title,
                        Url = x.Url,
                        ParentId = x.ParentId,
                        Seq = x.Seq
                    },
                    Icon = x.Icon,
                    Label = x.Label
                }).ToListAsync();

            List<TreeNode<MenuDto>> datas = menus
                .Where(x => x.Data.ParentId is null)
                .Select(x =>
                {
                    x.Children = GetChildren(menus, x);
                    return x;
                }).OrderBy(x => x.Data.Seq).ToList();

            return datas;
        }

        private static List<TreeNode<MenuDto>> GetChildren(List<TreeNode<MenuDto>> menus, TreeNode<MenuDto> parent)
        {
            return menus
                .Where(x => x.Data.ParentId == parent.Data.Id)
                .Select(x =>
                {
                    x.Children = GetChildren(menus, x);
                    return x;
                }).OrderBy(x => x.Data.Seq).ToList();
        }
        #endregion

        #region Create
        public async Task<OperationResult> Create(MenuDto dto)
        {
            if (await _context.Menu.AnyAsync(x => x.Id == dto.Id))
                return new OperationResult { IsSuccess = false, Message = "Menu đã tồn tại. Vui lòng kiểm tra lại " };

            Menu menu = new()
            {
                Id = dto.Id,
                Controller = dto.Controller,
                Icon = dto.Icon,
                Label = dto.Label,
                RouterLink = dto.RouterLink,
                Url = dto.Url,
                Target = dto.Target,
                Badge = dto.Badge,
                Title = dto.Title,
                ParentId = dto.ParentId,
                Seq = dto.Seq,
                Visible = dto.Visible,
                Separator = dto.Separator,
                Class = dto.Class
            };
            try
            {
                await _context.Menu.AddAsync(menu);

                await _context.SaveChangesAsync();
                return new OperationResult { IsSuccess = true };
            }
            catch
            {
                return new OperationResult { IsSuccess = false };
            }
        }
        #endregion

        #region Update
        public async Task<OperationResult> Update(MenuDto dto)
        {
            Menu menu = await _context.Menu.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (menu is null)
                return new OperationResult { IsSuccess = false, Message = "Menu không tồn tại. Vui lòng kiểm tra lại " };

            menu.Icon = dto.Icon;
            menu.Label = dto.Label;
            menu.RouterLink = dto.RouterLink;
            menu.Url = dto.Url;
            menu.Target = dto.Target;
            menu.Badge = dto.Badge;
            menu.Title = dto.Title;
            menu.ParentId = dto.ParentId;
            menu.Seq = dto.Seq;
            menu.Visible = dto.Visible;
            menu.Separator = dto.Separator;
            try
            {
                _context.Menu.Update(menu);

                await _context.SaveChangesAsync();
                return new OperationResult { IsSuccess = true };
            }
            catch
            {
                return new OperationResult { IsSuccess = false };
            }
        }
        #endregion

        #region Delete
        public async Task<OperationResult> Delete(long id)
        {
            Menu menu = await _context.Menu.FirstOrDefaultAsync(x => x.Id == id);
            if (menu is null)
                return new OperationResult { IsSuccess = false, Message = "Menu không tồn tại. Vui lòng kiểm tra lại " };

            List<Menu> menus = new() { menu };
            await GetMenuRecursive(menus, menu.Id);
            try
            {
                _context.Menu.RemoveRange(menus);

                await _context.SaveChangesAsync();
                return new OperationResult { IsSuccess = true };
            }
            catch
            {
                return new OperationResult { IsSuccess = false };
            }
        }

        private async Task GetMenuRecursive(List<Menu> menus, long parentId)
        {
            List<Menu> childs = await _context.Menu.Where(x => x.ParentId == parentId).ToListAsync();
            if (childs.Any())
            {
                menus.AddRange(childs);
                foreach (var child in childs)
                {
                    await GetMenuRecursive(menus, child.Id);
                }
            }
        }
        #endregion

        #region ConfigurationMenus
        public async Task<OperationResult> ConfigurationMenus(List<TreeNode<MenuDto>> nodes)
        {
            using var _transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                List<Menu> menuOlds = await _context.Menu.ToListAsync();
                if (menuOlds.Any())
                    _context.RemoveRange(menuOlds);

                int seq = 1;
                List<Menu> menus = nodes
                    .Where(x => x.Data.ParentId is null)
                    .Select(x => new Menu
                    {
                        Id = x.Data.Id,
                        Badge = x.Data.Badge,
                        Class = x.Data.Class,
                        Label = x.Data.Label,
                        RouterLink = x.Data.RouterLink,
                        Separator = x.Data.Separator,
                        Target = x.Data.Target,
                        Visible = x.Data.Visible,
                        Icon = x.Data.Icon,
                        Title = x.Data.Title,
                        Url = x.Data.Url,
                        ParentId = x.Data.ParentId,
                        Seq = seq++,
                        Controller = x.Data.Controller
                    }).ToList();

                nodes.Where(x => x.Data.ParentId is null).ForEach(x => ConfigurationMenuChilds(menus, x.Children));

                await _context.Menu.AddRangeAsync(menus);

                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();

                return new OperationResult { IsSuccess = true };
            }
            catch
            {
                await _transaction.RollbackAsync();
                return new OperationResult { IsSuccess = false };
            }
        }

        private static void ConfigurationMenuChilds(List<Menu> menus, List<TreeNode<MenuDto>> nodes)
        {
            int seq = 1;
            menus.AddRange(nodes
                .Select(x => new Menu
                {
                    Id = x.Data.Id,
                    Badge = x.Data.Badge,
                    Class = x.Data.Class,
                    Label = x.Data.Label,
                    RouterLink = x.Data.RouterLink,
                    Separator = x.Data.Separator,
                    Target = x.Data.Target,
                    Visible = x.Data.Visible,
                    Icon = x.Data.Icon,
                    Title = x.Data.Title,
                    Url = x.Data.Url,
                    ParentId = x.Data.ParentId,
                    Seq = seq++,
                    Controller = x.Data.Controller
                }).ToList());
        }
        #endregion

        #region GetListController
        public async Task<List<KeyValuePair<string, string>>> GetListController()
        {
            List<KeyValuePair<string, string>> result = await _context.Function
                .Where(x => x.IsMenu == true).AsNoTracking()
                .Select(x => new KeyValuePair<string, string>
                    (
                        x.Controller,
                        x.Title
                    )
                ).ToListAsync();

            return result;
        }
        #endregion
    }
}