using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Contracts
{
    public interface IFileSystem
    {
        bool FileExists(string path);
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        byte[] ReadAllBytes(string path);
        void WriteAllBytes(string path, byte[] data);
        IEnumerable<string> GetDirectoryFiles(string path,string searchPattern);
        void DeleteFile(string path);
    }
}
