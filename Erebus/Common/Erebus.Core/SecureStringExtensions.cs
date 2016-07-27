using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core
{
    public static class SecureStringExtensions
    {
        public static string ToActualString(this SecureString secureString)
        {
            IntPtr marshaledSecureString = SecureStringMarshal.SecureStringToGlobalAllocUnicode(secureString);
            string actualString = Marshal.PtrToStringUni(marshaledSecureString);
            Marshal.FreeHGlobal(marshaledSecureString);
            return actualString;
        }

        public static SecureString ToSecureString(this string actualString)
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
