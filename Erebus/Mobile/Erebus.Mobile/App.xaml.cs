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
using Erebus.Localization;
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

        public App(ContainerFactory containerFactory)
        {
            //InitializeComponent();
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
