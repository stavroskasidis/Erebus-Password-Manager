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
using Erebus.Mobile.Droid.PlatformImplementations;
using Erebus.Core.Mobile;
using Erebus.Core.Mobile.Contracts;
using Autofac;
using System.Diagnostics;

namespace Erebus.Mobile.Droid
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { Intent.ActionBootCompleted }, Priority = (int)IntentFilterPriority.LowPriority)]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Console.WriteLine($"Boot receiver started. Action: {intent.Action}");
            var containerFactory = new ContainerFactory();
            containerFactory.AddRegistrations(builder =>
            {
                var androidRegistrations = new AndroidPlatformServicesRegistrator();
                androidRegistrations.RegisterPlatformSpecificServices(builder);
            });

            var container = containerFactory.Build();
            var configurationManager = container.Resolve<IMobileConfigurationReader>();
            if (configurationManager.GetConfiguration().AlreadyInitialized)
            {
                if (configurationManager.GetConfiguration().ApplicationMode == ApplicationMode.Client)
                {
                    var synchronizer = container.Resolve<ISynchronizationServiceManager>();
                    synchronizer.StartSynchronizationService();
                }
            }

        }
    }
}