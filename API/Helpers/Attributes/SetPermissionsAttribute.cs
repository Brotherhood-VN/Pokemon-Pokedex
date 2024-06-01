using System.Security.Claims;
using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers.Attributes
{
    public class SetPermissionsAttribute
    {
        public readonly string controller;
        public readonly string action;

        public SetPermissionsAttribute(string _controller, string _action)
        {
            controller = _controller;
            action = _action;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            DBContext _db = context.HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            long accountId = long.Parse(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var accountRoles = _db.AccountRole.Where(x => x.AccountId == accountId).Select(x => x.RoleId).Distinct().ToList();
            var roles = _db.Role.Where(x => accountRoles.Contains(x.Id)).Select(x => x.Id).ToList();

            var roleFunctions = _db.RoleFunction
                .Include(x => x.Function)
                .Any(x => roles.Contains(x.RoleId) && x.Function.Controller == controller && x.Function.Action == action);

            var accountFunctions = _db.AccountFunction
                .Include(x => x.Function)
                .Any(x => x.AccountId == accountId && x.Function.Controller == controller && x.Function.Action == action);

            if (!roleFunctions && !accountFunctions)
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}