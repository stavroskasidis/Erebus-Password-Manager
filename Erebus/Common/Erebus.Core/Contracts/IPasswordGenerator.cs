using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Erebus.Core.Contracts
{
    public interface IPasswordGenerator
    {
        string GeneratePassword(int length,bool includeUppercase, bool includeNumbers, bool includeSymbols);
    }
}
