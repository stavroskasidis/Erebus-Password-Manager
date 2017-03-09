using Erebus.Core.Mobile.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using Erebus.Core.Contracts;

namespace Erebus.Core.Mobile.Implementations
{
    public class ApplicationContext : IApplicationContext
    {
        private string CurrentVaultName;
        private byte[] MasterPasswordBytes;
        private ISecureStringBinarySerializer SecureStringBinarySerializer;

        public ApplicationContext(ISecureStringBinarySerializer secureStringBinarySerializer)
        {
            SecureStringBinarySerializer = secureStringBinarySerializer;
        }

        public string GetCurrentVaultName()
        {
            return CurrentVaultName;
        }

        public SecureString GetMasterPassword()
        {
            if (MasterPasswordBytes == null) return null;
            return SecureStringBinarySerializer.FromByteArray(MasterPasswordBytes);
        }

        public void SetCurrentVaultName(string currentVaultName)
        {
            CurrentVaultName = currentVaultName;
        }

        public void SetMasterPassword(SecureString masterPassword)
        {
            MasterPasswordBytes = SecureStringBinarySerializer.ToByteArray(masterPassword);
        }
    }
}
