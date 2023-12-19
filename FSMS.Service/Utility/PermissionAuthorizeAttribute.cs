using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Utility
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private string[] _roles;
        public PermissionAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {

                var roleClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type.ToLower() == "role");
                if (_roles.FirstOrDefault(x => x.Trim().ToLower().Equals(roleClaim.Value.Trim().ToLower())) == null)
                {
                    context.Result = new ObjectResult("Forbidden") { StatusCode = 403, Value = "You are not allowed to access this function!" };
                }
                else
                {
                    return;
                }
            }
            else
            {
                context.Result = new StatusCodeResult(401);
            }
        }
    }
}
