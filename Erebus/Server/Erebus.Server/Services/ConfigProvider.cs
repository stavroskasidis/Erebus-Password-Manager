using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.Services
{
    public class ConfigProvider : IConfigProvider
    {
        private readonly IConfigurationRoot ConfigurationRoot;

        public ConfigProvider(IConfigurationRoot configurationRoot)
        {
            this.ConfigurationRoot = configurationRoot;
        }

        public ServerConfig GetConfiguration()
        {
            return new ServerConfig()
            {
                DisableSSLRequirement = bool.Parse(this.ConfigurationRoot["ServerConfiguration:DisableSSLRequirement"]),
                SmtpSettings = new SmtpSettings()
                {
                    SenderAddress = this.ConfigurationRoot["ServerConfiguration:SmtpSettings:SenderAddress"],
                    Host = this.ConfigurationRoot["ServerConfiguration:SmtpSettings:Host"],
                    Port = int.Parse(this.ConfigurationRoot["ServerConfiguration:SmtpSettings:Port"]),
                    UseSSL = bool.Parse(this.ConfigurationRoot["ServerConfiguration:SmtpSettings:UseSSL"])
                }
            };
        }
    }
}
