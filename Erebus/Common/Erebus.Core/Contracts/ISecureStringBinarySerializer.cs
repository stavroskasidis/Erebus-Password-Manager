using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Contracts
{
    public interface ISecureStringBinarySerializer
    {
        byte[] ToByteArray(SecureString secureString);
        SecureString FromByteArray(byte[] data);
    }
}
