using Autofac;
using Autofac.Core;
using Erebus.Core.Contracts;
using Erebus.Core.Implementations;
using Erebus.Core.Mobile.Contracts;
using Erebus.Core.Mobile.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile
{
    public class ContainerFactory
    {
        private ContainerBuilder ContainerBuilder;

        public ContainerFactory()
        {
            ContainerBuilder = new ContainerBuilder();

            //=== Common ===
            ContainerBuilder.RegisterType<MobileConfigurationReader>().As<IMobileConfigurationReader>().SingleInstance();
            ContainerBuilder.RegisterType<MobileConfigurationWriter>().As<IMobileConfigurationWriter>().SingleInstance();
            ContainerBuilder.RegisterType<UrlValidator>().As<IUrlValidator>().SingleInstance();
            ContainerBuilder.RegisterType<ApplicationContext>().As<IApplicationContext>().SingleInstance();
            ContainerBuilder.RegisterType<NavigationManager>().As<INavigationManager>().SingleInstance();
            ContainerBuilder.RegisterType<PresenterFactory>().As<IPresenterFactory>().SingleInstance();
            ContainerBuilder.RegisterType<AlertDisplayer>().As<IAlertDisplayer>().SingleInstance();
            ContainerBuilder.RegisterType<JsonSerializer>().As<ISerializer>().SingleInstance();
            ContainerBuilder.RegisterType<VaultFileRepositoryFactory>().As<IVaultRepositoryFactory>().SingleInstance();
            ContainerBuilder.RegisterType<AesCryptographer>().As<ISymetricCryptographer>().SingleInstance();
            ContainerBuilder.RegisterType<ClockProvider>().As<IClockProvider>().SingleInstance();
            ContainerBuilder.RegisterType<VaultFileMetadataHandler>().As<IVaultFileMetadataHandler>().SingleInstance();
            ContainerBuilder.RegisterType<SecureStringConverter>().As<ISecureStringConverter>().SingleInstance();
            ContainerBuilder.RegisterType<ByteArrayHelper>().As<IByteArrayHelper>().SingleInstance();
            ContainerBuilder.RegisterType<Synchronizer>().As<ISynchronizer>();
            ContainerBuilder.RegisterType<MobileSyncContext>().As<ISyncContext>();
            ContainerBuilder.RegisterType<PasswordGenerator>().As<IPasswordGenerator>();
            ContainerBuilder.RegisterType<ClipboardService>().As<IClipboardService>();
            ContainerBuilder.Register<IServerCommunicator>(x =>
            {
                return new ServerCommunicator(x.Resolve<IMobileConfigurationReader>().GetConfiguration().ServerUrl, x.Resolve<ISerializer>());
            });
            ContainerBuilder.Register<ISecureStringBinarySerializer>(x =>
            {
                string randomPassword = x.Resolve<IPasswordGenerator>().GeneratePassword(50, true, true, true, true);
                var secureStringConverter = x.Resolve<ISecureStringConverter>();
                return new SecureStringBinarySerializer(x.Resolve<ISymetricCryptographer>(), secureStringConverter.ToSecureString(randomPassword), x.Resolve<ISecureStringConverter>());
            }).SingleInstance();

        }

        public void AddRegistrations(Action<ContainerBuilder> builder)
        {
            builder(this.ContainerBuilder);
        }

        public IContainer Build()
        {
            return ContainerBuilder.Build();
        }
    }
}
