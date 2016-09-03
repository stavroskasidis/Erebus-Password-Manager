using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Erebus.Core.Server;

namespace Erebus.Server.Authorization
{
    public class VaultAuthorizationAttribute : Attribute, IAuthorizationFilter, IFilterMetadata
    {
        private IAuthorizationLogic AuthorizationLogic { get; set; }

        public VaultAuthorizationAttribute(IAuthorizationLogic authorizationLogic)
        {
           this.AuthorizationLogic = authorizationLogic;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (this.AuthorizationLogic.IsLoggedIn == false
                && context.Filters.Any(x=> x.GetType() == typeof(AllowAnonymousFilter)) == false)
            {
                var isAjax = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                if (isAjax)
                {
                    context.Result = new UnauthorizedResult();
                }
                else
                {
                    context.Result = new RedirectResult("/Login/Index");
                }
                
            }
        }
    }
}
