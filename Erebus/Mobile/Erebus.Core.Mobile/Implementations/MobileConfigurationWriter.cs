using Erebus.Core.Mobile.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Erebus.Core.Mobile.Implementations
{
    public class MobileConfigurationWriter : IMobileConfigurationWriter
    {
        private Application Application;

        public MobileConfigurationWriter(Application application)
        {
            this.Application = application;
        }

        public async Task SaveConfigurationAsync(MobileConfiguration configuration)
        {
            this.Application.Properties[nameof(configuration.ApplicationMode)] = configuration.ApplicationMode.ToString();
            this.Application.Properties[nameof(configuration.ServerUrl)] = configuration.ServerUrl;
            this.Application.Properties[nameof(configuration.Language)] = configuration.Language;
            this.Application.Properties[nameof(configuration.AlreadyInitialized)] = configuration.AlreadyInitialized.ToString();

            await this.Application.SavePropertiesAsync();
        }
    }
}
