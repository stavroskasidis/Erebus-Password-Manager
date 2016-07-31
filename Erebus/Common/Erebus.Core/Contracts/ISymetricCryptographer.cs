using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Contracts
{
    public interface ISymetricCryptographer
    {
        byte[] Encrypt(byte[] input, SecureString key);
        byte[] Decrypt(byte[] input, SecureString key);
        bool IsKeyValid(byte[] input, SecureString key);
    }
}
