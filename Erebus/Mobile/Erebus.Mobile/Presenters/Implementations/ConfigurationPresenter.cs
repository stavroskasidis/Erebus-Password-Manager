using Erebus.Mobile.Presenters.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erebus.Mobile.Views.Contracts;
using Erebus.Resources;
using Erebus.Core.Mobile.Contracts;
using Erebus.Core.Mobile;

namespace Erebus.Mobile.Presenters.Implementations
{
    public class ConfigurationPresenter : IConfigurationPresenter
    {
        private IConfigurationView View;
        private IUrlValidator UrlValidator;
        private IServerChecker ServerChecker;
        private IMobileConfigurationReader ConfigurationReader;
        private IMobileConfigurationWriter ConfigurationWriter;

        public ConfigurationPresenter(IConfigurationView view, IUrlValidator urlValidator, IServerChecker serverChecker,
                                       IMobileConfigurationReader configurationReader, IMobileConfigurationWriter configurationWriter)
        {
            this.View = view;
            this.UrlValidator = urlValidator;
            this.ServerChecker = serverChecker;
            this.ConfigurationReader = configurationReader;
            this.ConfigurationWriter = configurationWriter;


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

        public void OnSave()
        {
            if(this.View.SelectedApplicationMode == ApplicationMode.Client)
            {
                var serverUrl = this.View.ServerUrlInputText;
                bool isValid = this.UrlValidator.IsUrlValid(serverUrl);

                if (!isValid)
                {
                    View.ShowAlert(StringResources.InvalidUrl, StringResources.InvalidUrlMessage);
                    return;
                }
            }



            //todo navigate next
            throw new NotImplementedException();

        }

        public void OnSelectedApplicationModeChange()
        {
            this.View.ServerUrlInputEnabled = this.View.SelectedApplicationMode == ApplicationMode.Client;
        }
    }
}
