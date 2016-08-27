using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Contracts
{
    public interface ISyncContext
    {
        void Lock();
        void Release();
    }
}
