using Autofac;
using Erebus.Core.Contracts;
using Erebus.Core.Implementations;
using Erebus.Core.Mobile.Contracts;
using Erebus.Mobile.iOS;
using Erebus.Mobile.iOS.PlatformImplementations;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSPlatformServicesRegistrator))]

namespace Erebus.Mobile.iOS
{
    public class iOSPlatformServicesRegistrator : IPlatformServicesRegistrator
    {
        public void RegisterPlatformSpecificServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<iOSFileSystem>().As<IFileSystem>();
            //containerBuilder.RegisterType<iOSSyncContext>().As<ISyncContext>();
        }
    }
}
