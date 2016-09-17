using Autofac;
using Erebus.Core.Mobile.Contracts;
using Erebus.Core.Mobile.Implementations;
using Erebus.Mobile.Presenters.Contracts;
using Erebus.Mobile.Presenters.Implementations;
using Erebus.Mobile.Views.Contracts;
using Erebus.Mobile.Views.Implementations;
using Erebus.Resources;
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

        protected void RegisterDependencies(ContainerBuilder buider)
        {
            buider.RegisterType<MobileConfigurationReader>().As<IMobileConfigurationReader>();
            buider.RegisterType<MobileConfigurationWriter>().As<IMobileConfigurationWriter>();
            buider.RegisterType<ServerChecker>().As<IServerChecker>();
            buider.RegisterType<UrlValidator>().As<IUrlValidator>();
            buider.RegisterType<UrlValidator>().As<IUrlValidator>();
            buider.RegisterType<ApplicationContext>().As<IApplicationContext>().SingleInstance();
            buider.Register<Application>(x=> Application.Current);

            //=== Views/Presenters

            buider.RegisterType<ConfigurationPresenter>().As<IConfigurationPresenter>();
            buider.RegisterType<ConfigurationView>().As<IConfigurationView>();
        }

        public App()
        {
            InitializeComponent();

            var test = StringResources.ApplicationMode;
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            Container = containerBuilder.Build();

            var initializationPresenter = Container.Resolve<IConfigurationPresenter>();
            MainPage = initializationPresenter.GetView() as Page;
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
