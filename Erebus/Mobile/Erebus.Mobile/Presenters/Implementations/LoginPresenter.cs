using Erebus.Core.Contracts;
using Erebus.Core.Mobile;
using Erebus.Core.Mobile.Contracts;
using Erebus.Mobile.Presenters.Contracts;
using Erebus.Mobile.Views.Contracts;
using Erebus.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Mobile.Presenters.Implementations
{
    public class LoginPresenter : ILoginPresenter
    {
        private ILoginView View;
        private IMobileConfigurationReader MobileConfigurationReader;
        private INavigationManager NavigationManager;
        private IFileSystem FileSystem;
        private IVaultRepositoryFactory VaultRepositoryFactory;
        private ISynchronizer Synchronizer;
        private IAlertDisplayer AlertDisplayer;
        private ISecureStringConverter SecureStringConverter;
        private IApplicationContext ApplicationContext;

        public LoginPresenter(ILoginView view, IServerCommunicator serverCommunicator, IMobileConfigurationReader mobileConfigurationReader, INavigationManager navigationManager,
                              IFileSystem fileSystem, IVaultRepositoryFactory vaultRepositoryFactory, ISynchronizer synchronizer, IAlertDisplayer alertDisplayer,
                              ISecureStringConverter secureStringConverter, IApplicationContext applicationContext)
        {
            this.View = view;
            this.MobileConfigurationReader = mobileConfigurationReader;
            this.NavigationManager = navigationManager;
            this.FileSystem = fileSystem;
            this.Synchronizer = synchronizer;
            this.VaultRepositoryFactory = vaultRepositoryFactory;
            this.AlertDisplayer = alertDisplayer;
            this.SecureStringConverter = secureStringConverter;
            this.ApplicationContext = applicationContext;

            var config = MobileConfigurationReader.GetConfiguration();
            this.View.SyncButtonVisible = config.ApplicationMode == ApplicationMode.Client;
            var repository = VaultRepositoryFactory.CreateInstance();
            var vaults = repository.GetAllVaultNames();
            this.View.VaultNames = vaults;
            this.View.SelectedVaultName = vaults.FirstOrDefault();


            this.View.Login += OnLogin;
            this.View.NavigateToConfiguration += OnNavigateToConfiguration;
            this.View.Sync += OnSync;

        }

        public object GetView()
        {
            return View;
        }

        public void OnLogin()
        {
            if (string.IsNullOrWhiteSpace(this.View.MasterPasswordInputText))
            {
                this.AlertDisplayer.DisplayAlert(StringResources.PasswordRequired,StringResources.EnterVaultMasterPassword);
                return;
            }

            var vaultRepository = this.VaultRepositoryFactory.CreateInstance();
            var selectedVault = this.View.SelectedVaultName;
            var masterPassword =  this.SecureStringConverter.ToSecureString(this.View.MasterPasswordInputText);
            if(!vaultRepository.IsPasswordValid(selectedVault, masterPassword))
            {
                this.AlertDisplayer.DisplayAlert("", StringResources.IncorrectPassword);
                return;
            }

            this.ApplicationContext.SetCurrentVaultName(selectedVault);
            this.ApplicationContext.SetMasterPassword(masterPassword);

            this.NavigationManager.NavigateAsync<IVaultExplorerPresenter>();
        }

        public async void OnSync()
        {
            var config = MobileConfigurationReader.GetConfiguration();
            this.View.ActivityIndicatorText = StringResources.ContactingServer;
            this.View.DisableUI();
            this.View.ShowLoadingIndicator();

            this.Synchronizer.StatusUpdate += (message) => this.View.ActivityIndicatorText = message;
            bool success = await this.Synchronizer.Synchronize();
            if (success)
            {
                var repository = VaultRepositoryFactory.CreateInstance();
                var vaults = repository.GetAllVaultNames();
                this.View.VaultNames = vaults;
                this.View.SelectedVaultName = vaults.FirstOrDefault();
            }

            this.View.HideLoadingIndicator();
            this.View.EnableUI();
        }

        public void OnNavigateToConfiguration()
        {
            this.NavigationManager.NavigateAsync<IConfigurationPresenter>();
        }
    }
}
