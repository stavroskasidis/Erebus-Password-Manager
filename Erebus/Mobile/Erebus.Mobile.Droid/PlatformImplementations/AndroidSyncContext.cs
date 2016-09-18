using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Erebus.Core.Contracts;
using System.Threading;

namespace Erebus.Mobile.Droid.PlatformImplementations
{
    public class AndroidSyncContext : ISyncContext
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