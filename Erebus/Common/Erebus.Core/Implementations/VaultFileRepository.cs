using Erebus.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erebus.Model;
using System.Security;
using System.IO;

namespace Erebus.Core.Implementations
{
    public class VaultFileRepository : IVaultRepository
    {
        private IFileSystem FileSystem;
        private string VaultStorageFolder;
        private ISymetricCryptographer SymetricCryptographer;
        private ISerializer Serializer;

        public VaultFileRepository(IFileSystem fileSystem, string vaultStorageFolder, 
                                    ISymetricCryptographer symetricCryptographer, ISerializer serializer)
        {
            this.FileSystem = fileSystem;
            this.VaultStorageFolder = vaultStorageFolder;
            this.SymetricCryptographer = symetricCryptographer;
            this.Serializer = serializer;
        }


        public Vault GetVault(string vaultStorageName, SecureString masterPassword)
        {
            string path = Path.Combine(VaultStorageFolder, vaultStorageName);
            if (!this.FileSystem.FileExists(path)) throw new FileNotFoundException();

            byte[] fileBytes = this.FileSystem.ReadAllBytes(path);
            byte[] decryptedBytes = this.SymetricCryptographer.Decrypt(fileBytes, masterPassword);
            string fileData = Encoding.UTF8.GetString(decryptedBytes);
            return this.Serializer.Deserialize<Vault>(fileData);
        }

        public void SaveVault(string vaultStorageName, Vault vault, SecureString masterPassword)
        {
            string path = Path.Combine(VaultStorageFolder, vaultStorageName);
            string serialized = this.Serializer.Serialize(vault);
            byte[] fileBytes = Encoding.UTF8.GetBytes(serialized);
            byte[] encryptedBytes = this.SymetricCryptographer.Encrypt(fileBytes, masterPassword);
            this.FileSystem.WriteAllBytes(path,encryptedBytes);
        }

        public IEnumerable<string> GetAllVaultNames()
        {
            throw new NotImplementedException();
        }
    }
}
