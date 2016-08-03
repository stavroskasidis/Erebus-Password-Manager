using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erebus.Model;

namespace Erebus.Core.Implementations
{
    public class VaultManipulatorFactory : IVaultManipulatorFactory
    {
        private IClockProvider ClockProvider;

        public VaultManipulatorFactory(IClockProvider clockProvider)
        {
            GuardClauses.ArgumentIsNotNull(nameof(clockProvider), clockProvider);

            ClockProvider = clockProvider;
        }


        public IVaultManipulator CreateInstance(Vault vault)
        {
            return new VaultManipulator(vault, ClockProvider);
        }
    }
}
