using Erebus.Core.Contracts;
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
        private IFileSystem FileSystem;
        private ISerializer Serializer;

        public MobileConfigurationReader(IFileSystem fileSystem, ISerializer serializer)
        {
            this.FileSystem = fileSystem;
            this.Serializer = serializer;
        }

        public MobileConfiguration GetConfiguration()
        {
            if (FileSystem.FileExists(Constants.CONFIG_FILE_PATH))
            {
                var bytes = this.FileSystem.ReadAllBytes(Constants.CONFIG_FILE_PATH);
                var configuration = Serializer.Deserialize<MobileConfiguration>(GetString(bytes));

                return configuration;
            }
            else
            {
                return new MobileConfiguration();
            }
            //var config = new MobileConfiguration();
            //object applicationMode;
            //if(this.Application.Properties.TryGetValue(nameof(config.ApplicationMode), out applicationMode) && applicationMode!=null)
            //{
            //    config.ApplicationMode = (ApplicationMode)Enum.Parse(typeof(ApplicationMode),applicationMode.ToString());
            //}

            //object serverUrl;
            //if(this.Application.Properties.TryGetValue(nameof(config.ServerUrl), out serverUrl) && serverUrl != null) config.ServerUrl = serverUrl.ToString();

            //object language;
            //if(this.Application.Properties.TryGetValue(nameof(config.Language), out language) && language != null) config.Language = language.ToString();

            //object initialized;
            //if (this.Application.Properties.TryGetValue(nameof(config.AlreadyInitialized), out initialized) && initialized !=null) config.AlreadyInitialized = bool.Parse(initialized.ToString());

            //return config;
        }

        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
