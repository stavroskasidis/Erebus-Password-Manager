using Erebus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Contracts
{
    public interface IVaultHandlerFactory
    {
        IVaultHandler CreateInstance(Vault vault);
    }
}
