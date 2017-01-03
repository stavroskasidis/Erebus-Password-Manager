using Autofac;
using Erebus.Core.Mobile.Contracts;
using Erebus.Core.Implementations;
using Erebus.Core.Contracts;
using Xamarin.Forms;
using Erebus.Mobile.Droid;
using Erebus.Mobile.Droid.PlatformImplementations;

[assembly: Dependency(typeof(AndroidPlatformServicesRegistrator))]

namespace Erebus.Mobile.Droid
{
    public class AndroidPlatformServicesRegistrator : IPlatformServicesRegistrator
    {
        public void RegisterPlatformSpecificServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<AndroidFileSystem>().As<IFileSystem>();
            //containerBuilder.RegisterType<AndroidClipboardService>().As<IClipboardService>();
            containerBuilder.RegisterType<AndroidSynchronizationServiceManager>().As<ISynchronizationServiceManager>();
            //containerBuilder.RegisterType<AndroidSyncContext>().As<ISyncContext>();
        }
    }
}