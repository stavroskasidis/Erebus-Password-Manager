using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Implementations
{
    public class MobileSyncContext : ISyncContext
    {
        private static volatile bool IsLocked = false;

        public void Lock()
        {
            while (IsLocked)
            {
                Task.Delay(10).Wait();
            }

            IsLocked = true;
        }

        public void Release()
        {
            IsLocked = false;
        }
    }
}
