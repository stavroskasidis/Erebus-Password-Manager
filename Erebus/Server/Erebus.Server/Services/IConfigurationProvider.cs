using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.Services
{
    public interface IConfigProvider
    {
        ServerConfig GetConfiguration();
    }
}
