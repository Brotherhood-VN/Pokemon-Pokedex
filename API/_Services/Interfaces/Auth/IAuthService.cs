using API.Dtos;
using API.Helpers.Attributes;

namespace API._Services.Interfaces.Auth
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IAuthService
    {
        Task<UserReturnedDto> Login(UserForLoginParam userForLogin);
        Task<UserReturnedDto> RefreshToken(TokenRequestParam param);
        Task RevokeToken(TokenRequestParam param);

        // --------------------------------------------------------------------- //

        // Task<List<RoleUser>> GetRoles(RoleUserParams param);
        // Task<List<RoleAuth>> GetRoleByMenu(Guid accountId, string controller = "");
    }
}