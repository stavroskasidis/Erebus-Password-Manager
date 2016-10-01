using Erebus.Mobile.Presenters.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erebus.Mobile.Views.Contracts;
using Erebus.Resource;
using Erebus.Core.Mobile.Contracts;
using Erebus.Core.Mobile;

namespace Erebus.Mobile.Presenters.Implementations
{
    public class ConfigurationPresenter : IConfigurationPresenter
    {
        private IConfigurationView View;
        private IUrlValidator UrlValidator;
        private IMobileConfigurationReader ConfigurationReader;
        private IMobileConfigurationWriter ConfigurationWriter;
        private INavigationManager NavigationManager;
        private IAlertDisplayer AlertDisplayer;
        private ISynchronizationServiceManager SynchronizationServiceManager;

        public ConfigurationPresenter(IConfigurationView view, IUrlValidator urlValidator,
                                       IMobileConfigurationReader configurationReader, IMobileConfigurationWriter configurationWriter,
                                       INavigationManager navigationManager, IAlertDisplayer alertDisplayer, ISynchronizationServiceManager synchronizationServiceManager)
        {
            this.View = view;
            this.UrlValidator = urlValidator;
            this.ConfigurationReader = configurationReader;
            this.ConfigurationWriter = configurationWriter;
            this.NavigationManager = navigationManager;
            this.AlertDisplayer = alertDisplayer;
            this.SynchronizationServiceManager = synchronizationServiceManager;


            this.View.ApplicationModes = Enum.GetValues(typeof(ApplicationMode)).Cast<ApplicationMode>();
            var configuration = ConfigurationReader.GetConfiguration();
            this.View.SelectedApplicationMode = configuration.ApplicationMode;
            this.View.ServerUrlInputEnabled = configuration.ApplicationMode == ApplicationMode.Client;
            this.View.ServerUrlInputText = configuration.ServerUrl?? "https://";
            this.View.Save += OnSave;
            this.View.ApplicationModeChange += OnSelectedApplicationModeChange;
        }

        public object GetView()
        {
            return this.View;
        }

        public async void OnSave()
        {
            var configuration = new MobileConfiguration();
            configuration.ApplicationMode = this.View.SelectedApplicationMode;
            configuration.Language = "en"; //TODO make an option
            configuration.AlreadyInitialized = true;

            if (configuration.ApplicationMode == ApplicationMode.Client)
            {
                configuration.ServerUrl = this.View.ServerUrlInputText;
                bool isValid = this.UrlValidator.IsUrlValid(configuration.ServerUrl);

                if (!isValid)
                {
                    this.AlertDisplayer.DisplayAlert(StringResources.InvalidUrl, StringResources.InvalidUrlMessage);
                    return;
                }

                this.SynchronizationServiceManager.StartSynchronizationService();
            }
            else
            {
                this.SynchronizationServiceManager.StopSynchronizationService();
            }

            this.ConfigurationWriter.SaveConfiguration(configuration);
            await this.NavigationManager.NavigateByPopingCurrent<ILoginPresenter>();

        }

        public void OnSelectedApplicationModeChange()
        {
            this.View.ServerUrlInputEnabled = this.View.SelectedApplicationMode == ApplicationMode.Client;
        }
    }
}
