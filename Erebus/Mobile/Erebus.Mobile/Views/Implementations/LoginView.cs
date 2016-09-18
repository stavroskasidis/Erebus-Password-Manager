using Erebus.Mobile.Views.Contracts;
using Erebus.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Erebus.Mobile.Views.Implementations
{
    public class LoginView : ContentPage, ILoginView
    {
        private Picker VaultPicker = new Picker();
        private Button LoginButton = new Button();
        private Button SyncButton= new Button();
        private Label ActivityIndicatorMessage = new Label();
        private Button ConfigurationButton = new Button();
        private Entry MasterPasswordEntry = new Entry();
        private ActivityIndicator ActivityIndicator = new ActivityIndicator();

        public event Action Login;
        public event Action NavigateToConfiguration;
        public event Action Sync;

        public LoginView()
        {
            VaultPicker.Title = StringResources.Vault;
            MasterPasswordEntry.IsPassword = true;
            MasterPasswordEntry.Placeholder = StringResources.MasterPassword;
            LoginButton.Text = StringResources.Login;
            LoginButton.Clicked += (object sender, EventArgs e) => this.Login?.Invoke();
            ConfigurationButton.Text = StringResources.Configuration;
            ConfigurationButton.Clicked += (object sender, EventArgs e) => this.NavigateToConfiguration?.Invoke();
            SyncButton.Text = StringResources.Synchronize;
            SyncButton.Clicked += (object sender, EventArgs e) => this.Sync?.Invoke();
            ActivityIndicator.IsVisible = false;

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        SyncButton,
                        new Label
                        {
                            Text = "Login"
                        },
                        VaultPicker,
                        MasterPasswordEntry,
                        LoginButton,
                        ConfigurationButton,
                        ActivityIndicatorMessage,
                        ActivityIndicator
                    }
                }
            };
        }

        public string ActivityIndicatorText
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.ActivityIndicatorMessage.Text = value;
                });

            }
        }

        public string MasterPasswordInputText
        {
            get
            {
                return this.MasterPasswordEntry.Text;
            }
        }

        public string SelectedVaultName
        {
            get
            {
                return this.VaultPicker.Items[this.VaultPicker.SelectedIndex];
            }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.VaultPicker.SelectedIndex = this.VaultPicker.Items.IndexOf(value.ToString());
                });
            }
        }

        public IEnumerable<string> VaultNames
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.VaultPicker.Items.Clear();
                    foreach (var vault in value)
                    {
                        this.VaultPicker.Items.Add(vault);
                    }
                });
            }
        }

        public bool SyncButtonVisible
        {
            set
            {
                this.SyncButton.IsVisible = value;
            }
        }

        public void DisableUI()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.ConfigurationButton.IsEnabled = false;
                this.LoginButton.IsEnabled = false;
                this.MasterPasswordEntry.IsEnabled = false;
                this.VaultPicker.IsEnabled = false;
                this.SyncButton.IsEnabled = false;
            });
        }

        public void EnableUI()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.ConfigurationButton.IsEnabled = true;
                this.LoginButton.IsEnabled = true;
                this.MasterPasswordEntry.IsEnabled = true;
                this.VaultPicker.IsEnabled = true;
                this.SyncButton.IsEnabled = true;
            });
        }

        public void HideLoadingIndicator()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.ActivityIndicator.IsVisible = false;
                this.ActivityIndicator.IsRunning = false;
            });
        }

        public void ShowLoadingIndicator()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.ActivityIndicator.IsVisible = true;
                this.ActivityIndicator.IsRunning = true;
            });
        }
    }
}
