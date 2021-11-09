using Erebus.Core.Server.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Core.Server.Implementations
{
    public class ServerConfigurationReader : IServerConfigurationReader
    {
        private readonly IConfiguration ConfigurationRoot;

        public ServerConfigurationReader(IConfiguration configurationRoot)
        {
            this.ConfigurationRoot = configurationRoot;
        }

        public ServerConfiguration GetConfiguration()
        {
            return new ServerConfiguration()
            {
                DisableSSLRequirement = bool.Parse(this.ConfigurationRoot["ServerConfiguration:DisableSSLRequirement"]),
                VaultsFolder = this.ConfigurationRoot["ServerConfiguration:VaultsFolder"],
                Language = this.ConfigurationRoot["ServerConfiguration:Language"],
                BackupFolder = this.ConfigurationRoot["ServerConfiguration:BackupFolder"],
                SessionTimeoutMinutes = int.Parse(this.ConfigurationRoot["ServerConfiguration:SessionTimeoutMinutes"])
                //SmtpSettings = new SmtpSettings()
                //{
                //    SenderAddress = this.ConfigurationRoot["ServerConfiguration:SmtpSettings:SenderAddress"],
                //    Host = this.ConfigurationRoot["ServerConfiguration:SmtpSettings:Host"],
                //    Port = int.Parse(this.ConfigurationRoot["ServerConfiguration:SmtpSettings:Port"]),
                //    UseSSL = bool.Parse(this.ConfigurationRoot["ServerConfiguration:SmtpSettings:UseSSL"])
                //}
            };
        }
    }
}
