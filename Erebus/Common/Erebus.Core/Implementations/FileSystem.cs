using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Implementations
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string path)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadAllBytes(string path)
        {
            throw new NotImplementedException();
        }

        public void WriteAllBytes(string path, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
