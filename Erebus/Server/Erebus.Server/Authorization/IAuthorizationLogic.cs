using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.Authorization
{
    public interface IAuthorizationLogic
    {
        bool IsLoggedIn { get; }
    }
}
