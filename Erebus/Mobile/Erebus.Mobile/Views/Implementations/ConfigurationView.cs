using Erebus.Mobile.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using Erebus.Mobile.Presenters.Contracts;
using Erebus.Resources;
using Erebus.Core.Mobile;

namespace Erebus.Mobile.Views.Implementations
{
    public class ConfigurationView : ContentPage, IConfigurationView
    {
        private Picker ApplicationModesPicker = new Picker();
        private Button SaveButton = new Button();
        private Entry ServerUrlEntry = new Entry();

        public IEnumerable<ApplicationMode> ApplicationModes
        {
            set
            {
                foreach (string mode in value.Select(x => x.ToString()))
                {
                    this.ApplicationModesPicker.Items.Add(mode);
                }
            }
        }

        public ApplicationMode SelectedApplicationMode
        {
            get
            {
                return (ApplicationMode)Enum.Parse(typeof(ApplicationMode), this.ApplicationModesPicker.Items[this.ApplicationModesPicker.SelectedIndex]);
            }
            set
            {
                this.ApplicationModesPicker.SelectedIndex = this.ApplicationModesPicker.Items.IndexOf(value.ToString());
            }
        }

        public string ServerUrlInputText
        {
            get
            {
                return ServerUrlEntry.Text;
            }
            set
            {
                ServerUrlEntry.Text = value;
            }
        }

        public bool ServerUrlInputEnabled
        {
            get
            {
                return ServerUrlEntry.IsEnabled;
            }
            set
            {
                ServerUrlEntry.IsEnabled = value;
            }
        }

        public event Action Save;
        public event Action ApplicationModeChange;

        public ConfigurationView()
        {
            ApplicationModesPicker.Title = StringResources.ApplicationMode;
            ApplicationModesPicker.SelectedIndexChanged += (object sender, EventArgs e) => this.ApplicationModeChange?.Invoke();
            SaveButton.Text = StringResources.Save;
            SaveButton.Clicked += (object sender, EventArgs e) => this.Save?.Invoke();
            //ServerUrlEntry.Placeholder = StringResources.ServerUrl;
            
            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = 10,
                    Children =
                    {
                        new Label
                        {
                            Text = StringResources.Configuration,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            TextColor = Color.Black,
                            //VerticalOptions = LayoutOptions.Center,
                            HorizontalTextAlignment = TextAlignment.Center, // Center the text in the blue box.
                            VerticalTextAlignment = TextAlignment.Center, // Center the text in the blue box.
                        },
                        ApplicationModesPicker,
                        new Label
                        {
                            Text = StringResources.ServerUrl,
                            //FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            TextColor = Color.Black,
                            //VerticalOptions = LayoutOptions.Center,
                            //HorizontalTextAlignment = TextAlignment.Center, // Center the text in the blue box.
                            //VerticalTextAlignment = TextAlignment.Center, // Center the text in the blue box.
                        },
                        ServerUrlEntry,
                        SaveButton
                    }
                }
            };
        }

        public void ShowAlert(string title, string message)
        {
            this.DisplayAlert(title, message, StringResources.Ok);
        }
    }
}
