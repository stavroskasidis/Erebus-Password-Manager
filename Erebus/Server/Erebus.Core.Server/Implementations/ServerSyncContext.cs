using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Erebus.Core.Server.Implementations
{
    public class ServerSyncContext : ISyncContext
    {
        private EventWaitHandle waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, "erebus_sync_context_470B3552-D81A-4913-A928-74201783FE14");

        public void Lock()
        {
            waitHandle.WaitOne();
        }

        public void Release()
        {
            waitHandle.Set();
        }
    }
}
