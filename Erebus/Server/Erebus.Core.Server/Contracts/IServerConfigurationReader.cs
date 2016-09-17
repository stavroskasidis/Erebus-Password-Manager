using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Core.Server.Contracts
{
    public interface IServerConfigurationReader
    {
        ServerConfiguration GetConfiguration();
    }
}
