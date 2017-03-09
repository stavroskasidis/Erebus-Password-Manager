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
    public class MobileConfigurationWriter : IMobileConfigurationWriter
    {
        private IFileSystem FileSystem;
        private ISerializer Serializer;

        public MobileConfigurationWriter(IFileSystem fileSystem, ISerializer serializer)
        {
            this.FileSystem = fileSystem;
            this.Serializer = serializer;
        }

        public void SaveConfiguration(MobileConfiguration configuration)
        {
            var serialized = this.Serializer.Serialize(configuration);
            this.FileSystem.WriteAllBytes(Constants.CONFIG_FILE_PATH, GetBytes(serialized));
        }

        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
