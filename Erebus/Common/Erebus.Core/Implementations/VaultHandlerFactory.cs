using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erebus.Model;

namespace Erebus.Core.Implementations
{
    public class VaultHandlerFactory : IVaultHandlerFactory
    {
        private IClockProvider ClockProvider;

        public VaultHandlerFactory(IClockProvider clockProvider)
        {
            GuardClauses.ArgumentIsNotNull(nameof(clockProvider), clockProvider);

            ClockProvider = clockProvider;
        }


        public IVaultHandler CreateInstance(Vault vault)
        {
            return new VaultHandler(vault, ClockProvider);
        }
    }
}
