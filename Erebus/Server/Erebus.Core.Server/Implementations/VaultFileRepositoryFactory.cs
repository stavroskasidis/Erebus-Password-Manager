using Erebus.Core.Contracts;
using Erebus.Core.Implementations;
using Erebus.Core.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Core.Server.Implementations
{
    public class VaultFileRepositoryFactory : IVaultRepositoryFactory
    {
        private IFileSystem FileSystem;
        private ISymetricCryptographer SymetricCryptographer;
        private ISerializer Serializer;
        private IServerConfigurationReader ServerConfigurationReader;
        private IClockProvider ClockProvider;
        private IVaultFileMetadataHandler VaultFileMetadataHandler;

        public VaultFileRepositoryFactory(IFileSystem fileSystem, ISymetricCryptographer symetricCryptographer, ISerializer serializer, IServerConfigurationReader serverConfigurationReader,
                                          IClockProvider clockProvider, IVaultFileMetadataHandler vaultFileMetadataHandler)
        {
            GuardClauses.ArgumentIsNotNull(nameof(fileSystem), fileSystem);
            GuardClauses.ArgumentIsNotNull(nameof(symetricCryptographer), symetricCryptographer);
            GuardClauses.ArgumentIsNotNull(nameof(serializer), serializer);
            GuardClauses.ArgumentIsNotNull(nameof(serverConfigurationReader), serverConfigurationReader);
            GuardClauses.ArgumentIsNotNull(nameof(clockProvider), clockProvider);
            GuardClauses.ArgumentIsNotNull(nameof(vaultFileMetadataHandler), vaultFileMetadataHandler);

            this.FileSystem = fileSystem;
            this.SymetricCryptographer = symetricCryptographer;
            this.Serializer = serializer;
            this.ServerConfigurationReader = serverConfigurationReader;
            this.ClockProvider = clockProvider;
            this.VaultFileMetadataHandler = vaultFileMetadataHandler;
        }

        public IVaultRepository CreateInstance()
        {
            var fileRepository = new VaultFileRepository(FileSystem, ServerConfigurationReader.GetConfiguration().VaultsFolder, Constants.VAULT_FILE_NAME_EXTENSION, 
                                                        SymetricCryptographer, Serializer, ClockProvider, VaultFileMetadataHandler);
            if (string.IsNullOrWhiteSpace(ServerConfigurationReader.GetConfiguration().BackupFolder))
            {
                //No backup
                return fileRepository;
            }
            else
            {
                return new VaultFileBackupRepositoryDecorator(fileRepository, ServerConfigurationReader.GetConfiguration().BackupFolder, Constants.VAULT_FILE_NAME_EXTENSION,
                                                        Serializer, FileSystem, SymetricCryptographer, ClockProvider, VaultFileMetadataHandler);
            }
        }
    }
}

