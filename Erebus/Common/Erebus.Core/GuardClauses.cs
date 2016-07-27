using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core
{
    public static class GuardClauses
    {
        public static void ArgumentIsNotNull(string argumentName, object argument)
        {
            if (argument == null) throw new ArgumentNullException(argumentName);
        }
    }
}
