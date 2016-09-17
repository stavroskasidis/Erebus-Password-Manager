using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Contracts
{
    public interface IApplicationContext
    {
        SecureString GetMasterPassword();
        void SetMasterPassword(SecureString masterPassword);

        string GetCurrentVaultName();
        void SetCurrentVaultName(string currentVaultName);
    }
}
