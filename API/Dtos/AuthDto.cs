using API.Dtos.Systems;

namespace API.Dtos
{
    public class AuthDto
    {
        public long AccountId { get; set; }
        public string Avartar { get; set; }
        public string Type { get; set; }

        public string FullName { get; set; }
    }

    public class UserForLoginParam
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class TokenRequestParam
    {
        public string Token { get; set; }
    }

    public class UserReturnedDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Area { get; set; }
        public AuthDto User { get; set; }
        public List<MenuDto> Menus { get; set; }
    }
}