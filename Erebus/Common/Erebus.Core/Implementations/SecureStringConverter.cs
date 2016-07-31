using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Implementations
{
    public class SecureStringConverter : ISecureStringConverter
    {

        public string ToString(SecureString secureString)
        {
            IntPtr marshaledSecureString = SecureStringMarshal.SecureStringToGlobalAllocUnicode(secureString);
            string actualString = Marshal.PtrToStringUni(marshaledSecureString);
            Marshal.FreeHGlobal(marshaledSecureString);
            return actualString;
        }

        public SecureString ToSecureString(string actualString)
        {
            SecureString secureString = new SecureString();
            foreach(char c in actualString)
            {
                secureString.AppendChar(c);
            }
            secureString.MakeReadOnly();
            return secureString;
        }
    }
}
