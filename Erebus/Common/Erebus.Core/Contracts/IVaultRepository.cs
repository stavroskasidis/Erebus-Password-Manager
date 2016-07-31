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
        bool IsPasswordValid(string vaultName, SecureString masterPassword);
        Vault GetVault(string vaultName, SecureString masterPassword);
        void SaveVault(Vault vault, SecureString masterPassword);
        IEnumerable<string> GetAllVaultNames();
    }
}
