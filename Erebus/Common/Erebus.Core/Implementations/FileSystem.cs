using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Implementations
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string path)
        {
            GuardClauses.ArgumentIsNotNull(nameof(path), path);
            return File.Exists(path);
        }

        public bool DirectoryExists(string path)
        {
            GuardClauses.ArgumentIsNotNull(nameof(path), path);

            return Directory.Exists(path);
        }

        public byte[] ReadAllBytes(string path)
        {
            GuardClauses.ArgumentIsNotNull(nameof(path), path);

            return File.ReadAllBytes(path);
        }

        public void WriteAllBytes(string path, byte[] data)
        {
            GuardClauses.ArgumentIsNotNull(nameof(path), path);
            GuardClauses.ArgumentIsNotNull(nameof(data), data);

            File.WriteAllBytes(path, data);
        }

        public IEnumerable<string> GetDirectoryFiles(string path, string searchPattern)
        {
            GuardClauses.ArgumentIsNotNull(nameof(path), path);
            GuardClauses.ArgumentIsNotNull(nameof(searchPattern), searchPattern);

            return Directory.GetFiles(path, searchPattern).Select(filePath => Path.GetFileName(filePath));
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void DeleteFile(string path)
        {
            GuardClauses.ArgumentIsNotNull(nameof(path), path);
            File.Delete(path);
        }
    }
}
