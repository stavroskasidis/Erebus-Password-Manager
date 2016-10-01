using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Erebus.Mobile.UWP.Common.PlatformImplementations
{
    public class UWPFileSystem : IFileSystem
    {
        public void CreateDirectory(string path)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            storageFolder.CreateFolderAsync(path).AsTask().Wait();
        }

        public void DeleteFile(string path)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var task = storageFolder.GetFileAsync(path).AsTask();
            var storageFile = task.ConfigureAwait(false).GetAwaiter().GetResult();
            storageFile.DeleteAsync().AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public bool DirectoryExists(string path)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            try
            {
                var folder = storageFolder.GetFolderAsync(path).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                return folder != null;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        public bool FileExists(string path)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            try
            {
                var file = storageFolder.GetFileAsync(path).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                return file != null;
            }
            catch(FileNotFoundException)
            {
                return false;
            }
        }

        public IEnumerable<string> GetDirectoryFiles(string path, string searchPattern)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var folder = storageFolder.GetFolderAsync(path).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            var allFiles = folder.GetFilesAsync().AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            var files = allFiles.Where(x => x.FileType == searchPattern.Replace("*", ""));
            return files.Select(x => x.Name);
        }

        public void MoveFile(string sourcePath, string destinationPath)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var source = storageFolder.GetFileAsync(sourcePath).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            var destinationFolderPath = Path.GetDirectoryName(destinationPath);
            var destinationFolder = storageFolder.GetFolderAsync(destinationFolderPath).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            source.MoveAsync(destinationFolder).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public byte[] ReadAllBytes(string path)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var file = storageFolder.GetFileAsync(path).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            var buffer = FileIO.ReadBufferAsync(file).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
            {
                byte[] fileBytes = new byte[buffer.Length];
                dataReader.ReadBytes(fileBytes);
                return fileBytes;
            }
        }

        public void WriteAllBytes(string path, byte[] data)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var file = storageFolder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();

            FileIO.WriteBytesAsync(file, data).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
