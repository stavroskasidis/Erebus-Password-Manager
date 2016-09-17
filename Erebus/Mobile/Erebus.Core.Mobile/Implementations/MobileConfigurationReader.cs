using Erebus.Core.Mobile.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Erebus.Core.Mobile.Implementations
{
    public class MobileConfigurationReader : IMobileConfigurationReader
    {
        private Application Application;

        public MobileConfigurationReader(Application application)
        {
            this.Application = application;
        }

        public MobileConfiguration GetConfiguration()
        {
            var config = new MobileConfiguration();
            object applicationMode;
            if(this.Application.Properties.TryGetValue(nameof(config.ApplicationMode), out applicationMode))
            {
                config.ApplicationMode = (ApplicationMode)Enum.Parse(typeof(ApplicationMode),applicationMode.ToString());
            }

            object serverUrl;
            if(this.Application.Properties.TryGetValue(nameof(config.ServerUrl), out serverUrl)) config.ServerUrl = serverUrl.ToString();

            object language;
            if(this.Application.Properties.TryGetValue(nameof(config.Language), out language)) config.Language = language.ToString();

            return config;
        }
    }
}
