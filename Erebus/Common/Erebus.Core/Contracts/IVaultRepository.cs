using Erebus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Contracts
{
    public interface IVaultRepository
    {
        Vault GetVault(string vaultStorageName, SecureString masterPassword);
        void SaveVault(string vaultStorageName, Vault vault, SecureString masterPassword);
        IEnumerable<string> GetAllVaultNames();
    }
}
