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
    public class VaultFileBackupRepositoryDecorator : IVaultRepository
    {
        private IVaultRepository VaultRepository;
        private string VaultBackupFolder;
        private string VaultFileExtension;
        private IFileSystem FileSystem;
        private ISerializer Serializer;
        private ISymetricCryptographer SymetricCryptographer;
        private IClockProvider ClockProvider;
        private IVaultFileMetadataHandler VaultFileMetadataHandler;

        public VaultFileBackupRepositoryDecorator(IVaultRepository decoratedVaultRepository, string vaultBackupFolder,
                                                  string vaultFileExtension, ISerializer serializer, IFileSystem fileSystem,
                                                  ISymetricCryptographer symetricCryptographer, IClockProvider clockProvider,
                                                  IVaultFileMetadataHandler vaultFileMetadataHandler)
        {
            GuardClauses.ArgumentIsNotNull(nameof(decoratedVaultRepository), decoratedVaultRepository);
            GuardClauses.ArgumentIsNotNull(nameof(vaultBackupFolder), vaultBackupFolder);
            GuardClauses.ArgumentIsNotNull(nameof(vaultFileExtension), vaultFileExtension);
            if (vaultFileExtension.StartsWith(".") == false) throw new ArgumentException("File extension string must start with a dot. E.g. \".extension\"", nameof(vaultFileExtension));
            GuardClauses.ArgumentIsNotNull(nameof(symetricCryptographer), symetricCryptographer);
            GuardClauses.ArgumentIsNotNull(nameof(clockProvider), clockProvider);
            GuardClauses.ArgumentIsNotNull(nameof(vaultFileMetadataHandler), vaultFileMetadataHandler);

            this.VaultRepository = decoratedVaultRepository;
            this.VaultBackupFolder = vaultBackupFolder;
            this.VaultFileExtension = vaultFileExtension;
            this.Serializer = serializer;
            this.FileSystem = fileSystem;
            this.SymetricCryptographer = symetricCryptographer;
            this.ClockProvider = clockProvider;
            this.VaultFileMetadataHandler = vaultFileMetadataHandler;
        }

        public IEnumerable<string> GetAllVaultNames()
        {
            return VaultRepository.GetAllVaultNames();
        }

        public Vault GetVault(string vaultName, SecureString masterPassword)
        {
            return VaultRepository.GetVault(vaultName, masterPassword);
        }

        public bool IsPasswordValid(string vaultName, SecureString masterPassword)
        {
            return VaultRepository.IsPasswordValid(vaultName, masterPassword);
        }

        public void SaveVault(Vault vault, SecureString masterPassword)
        {
            GuardClauses.ArgumentIsNotNull(nameof(vault), vault);
            GuardClauses.ArgumentIsNotNull(nameof(masterPassword), masterPassword);

            VaultRepository.SaveVault(vault, masterPassword);

            if (FileSystem.DirectoryExists(VaultBackupFolder) == false)
            {
                FileSystem.CreateDirectory(VaultBackupFolder);
            }

            string vaultBackupFile = Path.ChangeExtension(Path.Combine(VaultBackupFolder, vault.Name), VaultFileExtension);

            //string serialized = this.Serializer.Serialize(vault);  --
            //byte[] fileBytes = Encoding.UTF8.GetBytes(serialized); --
            //byte[] encryptedBytes = this.SymetricCryptographer.Encrypt(fileBytes, masterPassword); --
            //this.FileSystem.WriteAllBytes(vaultBackupFile, encryptedBytes);

            string serialized = this.Serializer.Serialize(vault);
            byte[] vaultBytes = Encoding.UTF8.GetBytes(serialized);
            byte[] encryptedVaultBytes = this.SymetricCryptographer.Encrypt(vaultBytes, masterPassword);
            byte[] fileBytes = VaultFileMetadataHandler.AddMetadataHeader(new VaultMetadata
            {
                CreateLocation = vault.CreateLocation,
                Version = vault.Version
            }, encryptedVaultBytes);
            this.FileSystem.WriteAllBytes(vaultBackupFile, fileBytes);
        }

        public void DeleteVault(string vaultName)
        {
            GuardClauses.ArgumentIsNotNull(nameof(vaultName), vaultName);

            VaultRepository.DeleteVault(vaultName);

            string vaultBackupFile = Path.ChangeExtension(Path.Combine(VaultBackupFolder, vaultName), VaultFileExtension);
            if (this.FileSystem.FileExists(vaultBackupFile))
            {
                string deletedBackupFile = Path.ChangeExtension(Path.Combine(VaultBackupFolder,
                    $"{vaultName}_deleted_at_{ClockProvider.GetNow().ToString("yyyy_MM_dd_hh_mm_ss")}"), VaultFileExtension);
                this.FileSystem.MoveFile(vaultBackupFile, deletedBackupFile);
            }

        }

        public bool VaultExists(string vaultName)
        {
            return VaultRepository.VaultExists(vaultName);
        }

        public VaultMetadata GetVaultMetadata(string vaultName)
        {
            return VaultRepository.GetVaultMetadata(vaultName);
        }
    }
}
