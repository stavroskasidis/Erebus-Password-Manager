using Autofac;
using Erebus.Core.Contracts;
using Erebus.Core.Mobile.Contracts;
using Erebus.Mobile.UWP;
using Erebus.Mobile.UWP.PlatformImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(UWPPlatformServicesRegistrator))]

namespace Erebus.Mobile.UWP
{
    public class UWPPlatformServicesRegistrator : IPlatformServicesRegistrator
    {
        public void RegisterPlatformSpecificServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<UWPFileSystem>().As<IFileSystem>();
            containerBuilder.RegisterType<UWPClipboardService>().As<IClipboardService>();
            //containerBuilder.RegisterType<UWPSyncContext>().As<ISyncContext>();
        }
    }
}
