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
    public class EntryDetailsView : ContentPage, IEntryDetailsView
    {

        public event Action CopyPasswordToClipboard;
        public event Action ShowHidePassword;

        private Label TitleLabel = new Label();
        private Label UserNameLabel = new Label();
        private Entry PasswordEntry = new Entry();
        private Button PasswordShowHideButton = new Button();
        private Button PasswordCopyButton = new Button();
        private Label UrlLabel = new Label();
        private Label DescriptionLabel = new Label();

        public EntryDetailsView()
        {
            PasswordEntry.IsEnabled = false;
            PasswordEntry.IsPassword = true;
            PasswordShowHideButton.Text = StringResources.ShowHide;
            PasswordCopyButton.Text = StringResources.Copy;
            this.PasswordShowHideButton.Clicked += (object sender, EventArgs e) => ShowHidePassword?.Invoke();
            this.PasswordCopyButton.Clicked += (object sender, EventArgs e) => CopyPasswordToClipboard?.Invoke();

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                            Text = StringResources.Title
                        },
                        TitleLabel,

                        new Label
                        {
                            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                            Text = StringResources.UserName
                        },
                        UserNameLabel,

                        new Label
                        {
                            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                            Text = StringResources.Password
                        },
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children=
                            {
                                PasswordEntry,
                                PasswordShowHideButton,
                                PasswordCopyButton
                            }
                        },


                        new Label
                        {
                            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                            Text = StringResources.Url
                        },
                        UrlLabel,

                        new Label
                        {
                            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                            Text = StringResources.Description
                        },
                        DescriptionLabel,
                    }
                }
            };
        }

        public Model.Entry Entry
        {
            set
            {
                this.TitleLabel.Text = value.Title;
                this.UserNameLabel.Text = value.UserName;
                this.PasswordEntry.Text = value.Password;
                this.UrlLabel.Text = value.Url;
                this.DescriptionLabel.Text = value.Description;
            }
        }

        public bool IsPasswordVisible
        {
            get
            {
                return !this.PasswordEntry.IsPassword;
            }

            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.PasswordEntry.IsPassword = !value;
                });
            }
        }
    }
}
