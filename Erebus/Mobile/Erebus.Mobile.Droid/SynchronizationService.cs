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
using System.Threading.Tasks;
using Erebus.Core.Mobile;
using Autofac.Core;
using Erebus.Core.Mobile.Contracts;
using Autofac;
using System.Threading;

namespace Erebus.Mobile.Droid
{
    [Service]
    public class SynchronizationService : IntentService
    {
        public SynchronizationService() : base("SynchronizationService")
        {
        }

        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                Console.WriteLine("Service Started");
                var containerFactory = new ContainerFactory();
                containerFactory.AddRegistrations(builder =>
                {
                    var androidRegistrations = new AndroidPlatformServicesRegistrator();
                    androidRegistrations.RegisterPlatformSpecificServices(builder);
                });

                var container = containerFactory.Build();
                var synchronizer = container.Resolve<ISynchronizer>();
                
                var serverOnline = synchronizer.Synchronize().Result;
                Console.WriteLine($"Service Synchronization Finished - Result: {serverOnline}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //public override IBinder OnBind(Intent intent)
        //{
        //    return null;
        //}

        //private volatile bool IsStopped;

        //private void Wait(double waitTimeMiliseconds)
        //{
        //    var timeLeft = waitTimeMiliseconds;
        //    do
        //    {
        //        Task.Delay(1000).Wait();
        //        timeLeft -= 1000;
        //    }
        //    while (this.IsStopped == false && timeLeft > 0);

        //}

        //[return: GeneratedEnum]
        //public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        //{
        //    var t = new Thread(() =>
        //    {
        //        var containerFactory = new ContainerFactory();
        //        var container = containerFactory.Build();
        //        var synchronizer = container.Resolve<ISynchronizer>();

        //        try
        //        {
        //            var serverOnline = synchronizer.Synchronize().Result;
        //        }
        //        catch (Exception ex)
        //        {
        //        }

        //        StopSelf();
        //    });

        //    t.Start();

        //    return StartCommandResult.Sticky;
        //}

        //public override void OnDestroy()
        //{
        //    base.OnDestroy();
        //}
    }
}