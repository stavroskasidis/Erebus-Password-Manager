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
        private string VaultFileExtension;
        private ISymetricCryptographer SymetricCryptographer;
        private ISerializer Serializer;
        private IClockProvider ClockProvider;

        public VaultFileRepository(IFileSystem fileSystem, string vaultStorageFolder, string vaultFileExtension,
                                    ISymetricCryptographer symetricCryptographer, ISerializer serializer, IClockProvider clockProvider)
        {
            GuardClauses.ArgumentIsNotNull(nameof(fileSystem), fileSystem);
            GuardClauses.ArgumentIsNotNull(nameof(vaultStorageFolder), vaultStorageFolder);
            GuardClauses.ArgumentIsNotNull(nameof(vaultFileExtension), vaultFileExtension);
            GuardClauses.ArgumentIsNotNull(nameof(symetricCryptographer), symetricCryptographer);
            GuardClauses.ArgumentIsNotNull(nameof(serializer), serializer);
            GuardClauses.ArgumentIsNotNull(nameof(clockProvider), clockProvider);

            this.FileSystem = fileSystem;
            this.VaultStorageFolder = vaultStorageFolder;
            this.VaultFileExtension = vaultFileExtension;
            if (vaultFileExtension.StartsWith(".") == false) throw new ArgumentException("File extension string must start with a dot. E.g. \".extension\"", nameof(vaultFileExtension));
            this.SymetricCryptographer = symetricCryptographer;
            this.Serializer = serializer;
            this.ClockProvider = clockProvider;
        }

        public bool IsPasswordValid(string vaultName, SecureString masterPassword)
        {
            GuardClauses.ArgumentIsNotNull(nameof(vaultName), vaultName);
            GuardClauses.ArgumentIsNotNull(nameof(masterPassword), masterPassword);

            string path = Path.ChangeExtension(Path.Combine(VaultStorageFolder, vaultName), VaultFileExtension);
            if (!this.FileSystem.FileExists(path)) throw new FileNotFoundException();
            byte[] fileBytes = this.FileSystem.ReadAllBytes(path);
            return this.SymetricCryptographer.IsKeyValid(fileBytes, masterPassword);
        }

        public Vault GetVault(string vaultName, SecureString masterPassword)
        {
            GuardClauses.ArgumentIsNotNull(nameof(vaultName), vaultName);
            GuardClauses.ArgumentIsNotNull(nameof(masterPassword), masterPassword);

            string path = Path.ChangeExtension(Path.Combine(VaultStorageFolder, vaultName),VaultFileExtension);
            if (!this.FileSystem.FileExists(path)) throw new FileNotFoundException();

            byte[] fileBytes = this.FileSystem.ReadAllBytes(path);
            byte[] decryptedBytes = this.SymetricCryptographer.Decrypt(fileBytes, masterPassword);
            string fileData = Encoding.UTF8.GetString(decryptedBytes);
            return this.Serializer.Deserialize<Vault>(fileData);
        }
        

        public void SaveVault(Vault vault, SecureString masterPassword)
        {
            GuardClauses.ArgumentIsNotNull(nameof(vault), vault);
            GuardClauses.ArgumentIsNotNull(nameof(masterPassword), masterPassword);

            string path = Path.ChangeExtension(Path.Combine(VaultStorageFolder, vault.Name), VaultFileExtension);
            if (this.FileSystem.FileExists(path) == false)
            {
                vault.CreatedAt = ClockProvider.GetNow();
            }

            vault.UpdatedAt = ClockProvider.GetNow();
            string serialized = this.Serializer.Serialize(vault);
            byte[] fileBytes = Encoding.UTF8.GetBytes(serialized);
            byte[] encryptedBytes = this.SymetricCryptographer.Encrypt(fileBytes, masterPassword);
            this.FileSystem.WriteAllBytes(path,encryptedBytes);
        }

        public IEnumerable<string> GetAllVaultNames()
        {
            if(FileSystem.DirectoryExists(VaultStorageFolder) == false)
            {
                FileSystem.CreateDirectory(VaultStorageFolder);
            }

            var files = FileSystem.GetDirectoryFiles(VaultStorageFolder, "*" + VaultFileExtension);
            return files.Select(x => Path.GetFileNameWithoutExtension(x));
        }

        public bool VaultExists(string vaultName)
        {
            GuardClauses.ArgumentIsNotNull(nameof(vaultName), vaultName);

            string path = Path.ChangeExtension(Path.Combine(VaultStorageFolder, vaultName), VaultFileExtension);
            return this.FileSystem.FileExists(path);
        }

        public void DeleteVault(string vaultName)
        {
            GuardClauses.ArgumentIsNotNull(nameof(vaultName), vaultName);

            string path = Path.ChangeExtension(Path.Combine(VaultStorageFolder, vaultName), VaultFileExtension);
            this.FileSystem.DeleteFile(path);
        }
    }
}
