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
        private Mutex Mutex = new Mutex();

        public void Lock()
        {
            Mutex.WaitOne();
        }

        public void Release()
        {
            Mutex.ReleaseMutex();
        }
    }
}
