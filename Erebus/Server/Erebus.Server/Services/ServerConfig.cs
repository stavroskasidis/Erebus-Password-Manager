using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.Services
{
    public class ServerConfig
    {
        public bool DisableSSLRequirement { get; set; }
        //public SmtpSettings SmtpSettings { get; set; }
    }

    //public class SmtpSettings
    //{
    //    public string SenderAddress { get; set; }
    //    public string Host { get; set; }
    //    public int Port { get; set; }
    //    public bool UseSSL { get; set; }
    //}
}
