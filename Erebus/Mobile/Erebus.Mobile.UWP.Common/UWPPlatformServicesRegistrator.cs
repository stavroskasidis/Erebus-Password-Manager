using Autofac;
using Erebus.Core.Contracts;
using Erebus.Core.Mobile.Contracts;
using Erebus.Mobile.UWP;
using Erebus.Mobile.UWP.Common;
using Erebus.Mobile.UWP.Common.PlatformImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(UWPPlatformServicesRegistrator))]

namespace Erebus.Mobile.UWP.Common
{
    public class UWPPlatformServicesRegistrator : IPlatformServicesRegistrator
    {
        public void RegisterPlatformSpecificServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<UWPFileSystem>().As<IFileSystem>();
            containerBuilder.RegisterType<UWPClipboardService>().As<IClipboardService>();
            containerBuilder.RegisterType<UWPSynchronizationServiceManager>().As<ISynchronizationServiceManager>();
            //containerBuilder.RegisterType<UWPSyncContext>().As<ISyncContext>();
        }
    }
}
