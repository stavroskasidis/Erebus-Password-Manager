using Erebus.Core.Server;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.Authorization
{
    public class SessionAuthorizationLogic : IAuthorizationLogic
    {
        private IHttpContextAccessor HttpContextAccessor { get; set; }

        public SessionAuthorizationLogic(IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContextAccessor = httpContextAccessor;
        }

        public bool IsLoggedIn
        {
            get
            {
                var masterPassword = HttpContextAccessor.HttpContext.Session.Get(Constants.MASTER_PASSWORD_SESSION_KEY);
                var currentVault = HttpContextAccessor.HttpContext.Session.Get(Constants.CURRENT_VAULT_DESSION_KEY);
                return (masterPassword != null && currentVault != null);
            }
        }
    }
}
