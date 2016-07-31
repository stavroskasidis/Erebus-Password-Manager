using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erebus.Model;

namespace Erebus.Core.Implementations
{
    public class DefaultVaultFactory : IVaultFactory
    {
        private IVaultExplorerFactory VaultHandlerFactory;

        public DefaultVaultFactory(IVaultExplorerFactory vaultHandlerFactory)
        {
            this.VaultHandlerFactory = vaultHandlerFactory;
        }

        public Vault CreateVault(string vaultName)
        {
            var vault = new Vault();
            vault.Name = vaultName;
            var vaultHandler = VaultHandlerFactory.CreateInstance(vault);
            var group = new Group()
            {
                Name = "General"
            };

            vaultHandler.AddGroup(null, group);

            return vault;
        }
    }
}
