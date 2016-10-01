using Autofac;
using Erebus.Core.Contracts;
using Erebus.Core.Implementations;
using Erebus.Core.Mobile;
using Erebus.Core.Mobile.Contracts;
using Erebus.Core.Mobile.Implementations;
using Erebus.Mobile.Presenters.Contracts;
using Erebus.Mobile.Presenters.Implementations;
using Erebus.Mobile.Views.Contracts;
using Erebus.Mobile.Views.Implementations;
using Erebus.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Erebus.Mobile
{
    public partial class App : Application
    {
        public static IContainer Container { get; set; }

        //protected void RegisterDependencies(ContainerBuilder buider)
        //{
        //    //=== Common ===
        //    buider.RegisterType<MobileConfigurationReader>().As<IMobileConfigurationReader>().SingleInstance();
        //    buider.RegisterType<MobileConfigurationWriter>().As<IMobileConfigurationWriter>().SingleInstance();
        //    buider.RegisterType<UrlValidator>().As<IUrlValidator>().SingleInstance();
        //    buider.RegisterType<ApplicationContext>().As<IApplicationContext>().SingleInstance();
        //    buider.RegisterType<NavigationManager>().As<INavigationManager>().SingleInstance();
        //    buider.RegisterType<PresenterFactory>().As<IPresenterFactory>().SingleInstance();
        //    buider.RegisterType<AlertDisplayer>().As<IAlertDisplayer>().SingleInstance();
        //    buider.RegisterType<JsonSerializer>().As<ISerializer>().SingleInstance();
        //    buider.RegisterType<VaultFileRepositoryFactory>().As<IVaultRepositoryFactory>().SingleInstance();
        //    buider.RegisterType<AesCryptographer>().As<ISymetricCryptographer>().SingleInstance();
        //    buider.RegisterType<ClockProvider>().As<IClockProvider>().SingleInstance();
        //    buider.RegisterType<VaultFileMetadataHandler>().As<IVaultFileMetadataHandler>().SingleInstance();
        //    buider.RegisterType<SecureStringConverter>().As<ISecureStringConverter>().SingleInstance();
        //    buider.RegisterType<ByteArrayHelper>().As<IByteArrayHelper>().SingleInstance();
        //    buider.RegisterType<Synchronizer>().As<ISynchronizer>();
        //    buider.RegisterType<MobileSyncContext>().As<ISyncContext>();
        //    buider.RegisterType<PasswordGenerator>().As<IPasswordGenerator>();
        //    buider.Register<ISecureStringBinarySerializer>(x =>
        //    {
        //        string randomPassword = x.Resolve<IPasswordGenerator>().GeneratePassword(50, true, true, true, true);
        //        var secureStringConverter = x.Resolve<ISecureStringConverter>();
        //        return new SecureStringBinarySerializer(x.Resolve<ISymetricCryptographer>(), secureStringConverter.ToSecureString(randomPassword), x.Resolve<ISecureStringConverter>());
        //    }).SingleInstance();

        //    buider.Register<IServerCommunicator>(x =>
        //    {
        //        return new ServerCommunicator(x.Resolve<IMobileConfigurationReader>().GetConfiguration().ServerUrl, x.Resolve<ISerializer>());
        //    });
        //    buider.Register<Application>(x => Application.Current);
        //    buider.Register<IContainer>(x => Container);

        //    //=== Views/Presenters

        //    buider.RegisterType<ConfigurationPresenter>().As<IConfigurationPresenter>();
        //    buider.RegisterType<ConfigurationView>().As<IConfigurationView>();
        //    buider.RegisterType<LoginPresenter>().As<ILoginPresenter>();
        //    buider.RegisterType<LoginView>().As<ILoginView>();
        //    buider.RegisterType<VaultExplorerPresenter>().As<IVaultExplorerPresenter>();
        //    buider.RegisterType<VaultExplorerView>().As<IVaultExplorerView>();
        //    buider.RegisterType<EntryDetailsPresenter>().As<IEntryDetailsPresenter>();
        //    buider.RegisterType<EntryDetailsView>().As<IEntryDetailsView>();


        //    //=== Platform Specific
        //    var platformServicesRegistrator = DependencyService.Get<IPlatformServicesRegistrator>();
        //    platformServicesRegistrator.RegisterPlatformSpecificServices(buider);
        //}

        public App(ContainerFactory containerFactory)
        {
            InitializeComponent();

            //var containerBuilder = new ContainerBuilder();
            containerFactory.AddRegistrations(builder =>
            {
                builder.Register<Application>(x => Application.Current);
                builder.Register<IContainer>(x => Container);

                builder.RegisterType<ConfigurationPresenter>().As<IConfigurationPresenter>();
                builder.RegisterType<ConfigurationView>().As<IConfigurationView>();
                builder.RegisterType<LoginPresenter>().As<ILoginPresenter>();
                builder.RegisterType<LoginView>().As<ILoginView>();
                builder.RegisterType<VaultExplorerPresenter>().As<IVaultExplorerPresenter>();
                builder.RegisterType<VaultExplorerView>().As<IVaultExplorerView>();
                builder.RegisterType<EntryDetailsPresenter>().As<IEntryDetailsPresenter>();
                builder.RegisterType<EntryDetailsView>().As<IEntryDetailsView>();

                //=== Platform Specific
                var platformServicesRegistrator = DependencyService.Get<IPlatformServicesRegistrator>();
                platformServicesRegistrator.RegisterPlatformSpecificServices(builder);
            });
            
            Container = containerFactory.Build();


            var configurationManager = Container.Resolve<IMobileConfigurationReader>();
            var presenterFactory = Container.Resolve<IPresenterFactory>();
            var synchronizationServiceManager = Container.Resolve<ISynchronizationServiceManager>();

            if (configurationManager.GetConfiguration().AlreadyInitialized)
            {
                if(configurationManager.GetConfiguration().ApplicationMode == ApplicationMode.Client)
                {
                    synchronizationServiceManager.StartSynchronizationService();
                }
                var loginPresenter = presenterFactory.Create<ILoginPresenter>();
                MainPage = new NavigationPage(loginPresenter.GetView() as Page);
            }
            else
            {
                var configPresenter = presenterFactory.Create<IConfigurationPresenter>();
                MainPage = new NavigationPage(configPresenter.GetView() as Page);
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
