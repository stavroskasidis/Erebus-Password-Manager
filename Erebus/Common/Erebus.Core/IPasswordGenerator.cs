using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Erebus.Core
{
    public interface IPasswordGenerator
    {
        string GeneratePassword();
    }
}
