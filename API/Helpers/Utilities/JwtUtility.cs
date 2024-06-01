
using API.Dtos;
using API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Helpers.Utilities
{
    public class JwtUtility
    {
        public static string GenerateJwtToken(AuthDto user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(type: "Type", user.Type)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SettingsConfigUtility.GetCurrentSettings("AppSettings:Token")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SettingsConfigUtility.GetCurrentSettings("AppSettings:Token"));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.First(x => x.Type == "nameid").Value;

                return username;
            }
            catch
            {
                return null;
            }
        }

        public static RefreshToken GenerateRefreshToken(AuthDto user)
        {
            // Tạo Refrest Token có hạn là 3 ngày
            var rngCryptoServiceProvider = RandomNumberGenerator.Create();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.Now.AddDays(3),
                CreatedTime = DateTime.Now,
                AccountId = user.AccountId
            };

            return refreshToken;
        }
    }
}