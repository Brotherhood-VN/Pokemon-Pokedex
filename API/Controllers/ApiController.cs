using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    [Produces("application/json")]
    public class ApiController : ControllerBase
    {
        protected long UserId => long.Parse(new(User.FindFirst(ClaimTypes.NameIdentifier).Value));
        protected string UserName => User.FindFirst(ClaimTypes.Name).Value;
        protected static DateTime Now => DateTime.Now;
    }
}