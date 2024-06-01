using Microsoft.AspNetCore.Mvc;
using API._Services.Interfaces.Auth;
using API.Dtos;
using System.Security.Claims;

namespace API.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginParam userForLogin)
        {
            var result = await _authService.Login(userForLogin);

            if (result.User == null)
                return Unauthorized();

            return Ok(result);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestParam tokenRequest)
        {
            var result = await _authService.RefreshToken(tokenRequest);

            if (result.User == null)
                return Unauthorized();

            return Ok(result);
        }

        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] TokenRequestParam tokenRequest)
        {
            if (!string.IsNullOrEmpty(tokenRequest.Token))
                await _authService.RevokeToken(tokenRequest);

            return NoContent();
        }

        // [HttpGet("GetRoleByMenu")]
        // public async Task<IActionResult> GetRoleByMenu([FromQuery] string controller)
        // {
        //     return Ok(await _authService.GetRoleByMenu(long.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), controller));
        // }

        [HttpGet("DescryptUserPassword")]
        public static string GetPass([FromQuery] string pass)
        {
            return EncryptorUtility.DescryptUserPassword(pass);
        }
    }
}