using Erebus.Core.Mobile.Contracts;
using Erebus.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Erebus.Core.Mobile.Implementations
{
    public class AlertDisplayer : IAlertDisplayer
    {
        public Application Application { get; set; }

        public AlertDisplayer(Application application)
        {
            this.Application = application;
        }

        public void DisplayAlert(string title, string message)
        {
            this.Application.MainPage.DisplayAlert(title, message, StringResources.Ok);
        }
    }
}
