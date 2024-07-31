using API._Services.Interfaces.Auth;
using API.Data;
using API.Dtos;
using API.Models;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using API.Dtos.Systems;

namespace API._Services.Implementations.Auth
{
    public class AuthService : IAuthService
    {
        private readonly DBContext _context;

        public AuthService(DBContext context)
        {
            _context = context;
        }

        #region Login - đăng nhập hệ thống
        public async Task<UserReturnedDto> Login(UserForLoginParam userForLogin)
        {
            // Kiểm tra tên đăng nhập và mật khẩu
            if (string.IsNullOrEmpty(userForLogin.Username) || string.IsNullOrEmpty(userForLogin.Password))
            {
                return null;
            }

            // Tiến hành đăng nhập
            string password = EncryptorUtility.EncryptUserPassword(userForLogin.Password);
            AuthDto user = await _context.Account
                .Where(x =>
                    x.UserName.Trim() == userForLogin.Username.Trim() &&
                    x.Password.Trim() == password)
                .Include(x => x.AccountType)
                .Select(x => new AuthDto() { AccountId = x.Id, FullName = x.UserName, Type = x.AccountType.Title })
                .FirstOrDefaultAsync();

            // Không tồn tại username hoặc mật khẩu
            if (user == null)
                return null;

            // Tạo Token và Refresh Token
            string jwtToken = JwtUtility.GenerateJwtToken(user);
            RefreshToken refreshToken = JwtUtility.GenerateRefreshToken(user);

            // Xoá các Refresh Token hết hạn hoặc không hợp lệ
            await RemoveOldRefreshTokens(user);

            // Lưu Refersh Token mới vào DB
            _context.RefreshToken.Add(refreshToken);
            await _context.SaveChangesAsync();

            // Khởi tạo user trả về
            var userToReturn = new UserReturnedDto
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                Area = SettingsConfigUtility.GetCurrentSettings("AppSettings:Area"),
                User = user,
                // Menus = await GetMenus(user.AccountId)
            };

            return userToReturn;
        }
        #endregion

        #region RefreshToken
        public async Task<UserReturnedDto> RefreshToken(TokenRequestParam param)
        {
            var user = await GetUserByRefreshToken(param);

            if (user == null)
                return null;

            var refreshToken = await _context.RefreshToken.FirstOrDefaultAsync(x => x.Token == param.Token);

            // Trường hợp Token bị thu hồi rồi thì thu hồi tất cả Token con cháu
            if (refreshToken.IsRevoked)
                await RevokeDescendantRefreshTokens(refreshToken, user, $"Attempted reuse of revoked ancestor token: {param.Token}");

            // Trường hợp Token không hoạt động
            if (!refreshToken.IsActive)
                return null;

            // Tạo Refresh Token mới
            var newRefreshToken = await RotateRefreshToken(refreshToken, user);
            _context.RefreshToken.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            // Xoá Refresh Token cũ
            await RemoveOldRefreshTokens(user);

            // Tạo Token mới
            var jwtToken = JwtUtility.GenerateJwtToken(user);

            var userToReturn = new UserReturnedDto
            {
                Token = jwtToken,
                RefreshToken = newRefreshToken.Token,
                Area = SettingsConfigUtility.GetCurrentSettings("AppSettings:Area"),
                User = user,
                // Menus = await GetMenus(user.AccountId)
            };

            return userToReturn;
        }
        #endregion

        #region RemoveOldRefreshTokens
        private async Task RemoveOldRefreshTokens(AuthDto user)
        {
            // Lấy ra các Refresh Token hết hạn (sau 2 ngày) hoặc không hợp lệ
            var refreshTokens = await _context.RefreshToken
                .Where(x => x.AccountId == user.AccountId)
                .ToListAsync();

            refreshTokens = refreshTokens.Where(x => !x.IsActive && x.CreatedTime.Value.AddDays(2) <= DateTime.Now).ToList();

            // Xoá các Refresh Token
            _context.RefreshToken.RemoveRange(refreshTokens);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region GetUserByRefreshToken
        private async Task<AuthDto> GetUserByRefreshToken(TokenRequestParam param)
        {
            var accountId = await _context.RefreshToken
                .Where(x => x.Token == param.Token)
                .Select(x => x.AccountId)
                .FirstOrDefaultAsync();

            AuthDto user = await _context.Account
                .Where(x => x.Id == accountId)
                .Include(x => x.AccountType)
                .Select(x => new AuthDto() { AccountId = x.Id, FullName = x.UserName, Type = x.AccountType.Title })
                .FirstOrDefaultAsync();

            // Không tồn tại username hoặc mật khẩu
            if (user == null)
                return null;

            return user;
        }
        #endregion

        #region RevokeDescendantRefreshTokens
        private async Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, AuthDto user, string reason)
        {
            // Đệ quy tìm tất cả Refresh Token con cháu và thu hồi tất cả
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = await _context.RefreshToken.FirstOrDefaultAsync(x => x.Token == refreshToken.ReplacedByToken);

                if (childToken.IsActive)
                    await RevokeRefreshToken(childToken, reason);
                else
                    await RevokeDescendantRefreshTokens(childToken, user, reason);
            }
        }
        #endregion

        #region RevokeRefreshToken
        private async Task RevokeRefreshToken(RefreshToken token, string reason = null, string replacedByToken = null)
        {
            token.RevokedTime = DateTime.Now;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;

            _context.RefreshToken.Update(token);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region RotateRefreshToken
        private async Task<RefreshToken> RotateRefreshToken(RefreshToken refreshToken, AuthDto user)
        {
            var newRefreshToken = JwtUtility.GenerateRefreshToken(user);
            await RevokeRefreshToken(refreshToken, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }
        #endregion

        #region RevokeToken
        public async Task RevokeToken(TokenRequestParam param)
        {
            var userDto = await GetUserByRefreshToken(param);

            if (userDto != null)
            {
                var refreshToken = await _context.RefreshToken.FirstOrDefaultAsync(x => x.Token == param.Token);

                if (refreshToken.IsActive)
                    await RevokeRefreshToken(refreshToken, "Revoked without replacement");
            }
        }
        #endregion

        #region GetRoles
        /// <summary>
        /// Hàm xử lý lấy danh sách quyền
        /// </summary>
        /// <param name="checkedAll">
        /// Lấy danh sách quyền của tài khoản, checkedAll == true thì lấy hết
        /// </param>
        /// <param name="roleIds">roleIds là các nhóm quyền của tài khoản đó</param>
        /// <param name="accountId">accountId đang đăng nhập</param>
        /// <returns>Trả về danh sách quyền</returns>
        public async Task<List<RoleUser>> GetRoles(RoleUserParams param)
        {
            var functions = await _context.Function.Where(x => !x.Controller.Contains("Auth"))
                .Include(x => x.RoleFunctions)
                .Include(x => x.AccountFunctions)
                .OrderBy(x => x.Area).ThenBy(x => x.Controller).ThenBy(x => x.Action)
                .AsNoTracking().ToListAsync();

            if (!param.CheckedAll)
            {
                List<Function> funcByRoles = new();
                List<Function> funcByAccountId = new();
                if (param.RoleIds != null)
                    funcByRoles = functions.Where(x => x.RoleFunctions.Where(x => param.RoleIds.Contains(x.RoleId)).Any()).ToList();
                if (param.AccountId != null)
                {
                    funcByAccountId = functions.Where(x => x.AccountFunctions.Where(x => x.AccountId == param.AccountId).Any()).ToList();
                }

                functions = funcByRoles.Union(funcByAccountId).DistinctBy(x => x.Id).ToList();
            }

            List<RoleUser> roles = new();
            var funcGroup = functions.GroupBy(x => x.Controller).ToList();

            long index = 0;
            foreach (var con in funcGroup)
            {
                var func = functions.Where(x => x.Controller == con.Key).ToList();
                List<SubRoleUser> actions = new();
                foreach (var act in func)
                {
                    var item = new SubRoleUser
                    {
                        Action = act.Action,
                        Area = act.Area,
                        Controller = act.Controller,
                        Id = act.Id,
                        ParentId = index,
                        Title = act.Title,
                        Seq = act.Seq,
                        IsChecked = act.RoleFunctions.Any(x => x.FunctionId == act.Id && param.RoleIds.Contains(x.RoleId)) ||
                                    act.AccountFunctions.Any(x => x.FunctionId == act.Id && x.AccountId == param.AccountId),
                        IsDisabled = act.RoleFunctions.Any(x => x.FunctionId == act.Id && param.RoleIds.Contains(x.RoleId))
                    };
                    actions.Add(item);
                }

                RoleUser role = new()
                {
                    Id = index,
                    Controller = con.Key,
                    Title = func[0].Title,
                    Actions = actions.OrderBy(x => x.Seq).ToList(),
                    IsChecked = actions.Count(x => x.IsChecked) == func.Count,
                    IsDisabled = actions.Count(x => x.IsDisabled) == func.Count
                };

                index++;
                roles.Add(role);
            }

            return roles;
        }
        #endregion

        #region 
        /// <summary>
        /// Hàm xử lý lấy danh sách quyền theo menu
        /// </summary>
        /// <param name="accountId">Tài khoản đang đăng nhập</param>
        /// <param name="controller">Controller cần kiểm tra các quyền</param>
        /// <returns>Trả về danh sách các quyền của menu đó</returns>
        public async Task<List<RoleAuth>> GetRoleByMenu(long accountId, string controller = "")
        {
            var accountRoles = await _context.AccountRole.Where(x => x.AccountId == accountId).Select(x => x.RoleId).Distinct().ToListAsync();
            var roles = await _context.Role.Where(x => accountRoles.Contains(x.Id)).Select(x => x.Id).ToListAsync();

            var predicateRole = PredicateBuilder.New<RoleFunction>(x => roles.Contains(x.RoleId));
            var predicateAccount = PredicateBuilder.New<AccountFunction>(x => x.AccountId == accountId);

            if (!string.IsNullOrWhiteSpace(controller))
            {
                predicateRole.And(x => x.Function.Controller == controller);
                predicateAccount.And(x => x.Function.Controller == controller);
            }

            var roleFunctions = await _context.RoleFunction
                .Include(x => x.Function)
                .Where(predicateRole)
                .Select(x => x.Function)
                .AsNoTracking().ToListAsync();

            var accountFunctions = await _context.AccountFunction
                .Include(x => x.Function)
                .Where(predicateAccount)
                .Select(x => x.Function)
                .AsNoTracking().ToListAsync();

            List<RoleAuth> results = roleFunctions.Union(accountFunctions)
                .Select(x => new RoleAuth
                {
                    Id = x.Id,
                    Area = x.Area,
                    Controller = x.Controller,
                    Action = x.Action,
                    Title = x.Title,
                    IsActive = true
                })
                .OrderBy(x => x.Area).ThenBy(x => x.Controller).ThenBy(x => x.Action)
                .Distinct().ToList();

            return results;
        }
        #endregion

        #region GetMenus
        private async Task<List<MenuDto>> GetMenus(long accountId)
        {
            var roles = await GetRoleByMenu(accountId);
            var controllers = roles.Select(x => x.Controller).Distinct().ToList();

            var menus = await _context.Menu
                .Where(x => string.IsNullOrWhiteSpace(x.Controller) || controllers.Contains(x.Controller))
                .Select(x => new MenuDto
                {
                    Id = x.Id,
                    Badge = x.Badge,
                    Controller = x.Controller,
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
                }).AsNoTracking().ToListAsync();

            List<MenuDto> datas = menus
                .Where(x => !x.ParentId.HasValue)
                .Select(x =>
                {
                    x.Items = GetChildren(menus, x.Id);
                    return x;
                }).OrderBy(x => x.Seq).ToList();

            return datas.Where(x => !x.ParentId.HasValue && x.Items.Any()).ToList();
        }

        private static List<MenuDto> GetChildren(List<MenuDto> menus, long parentId)
        {
            return menus
                .Where(x => x.ParentId == parentId)
                .Select(x =>
                {
                    x.Items = GetChildren(menus, x.Id);
                    return x;
                }).OrderBy(x => x.Seq).ToList();
        }
        #endregion    
    }
}