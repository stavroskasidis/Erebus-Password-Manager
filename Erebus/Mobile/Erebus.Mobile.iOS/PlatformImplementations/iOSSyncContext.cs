using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Erebus.Mobile.iOS.PlatformImplementations
{
    public class iOSSyncContext : ISyncContext
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
