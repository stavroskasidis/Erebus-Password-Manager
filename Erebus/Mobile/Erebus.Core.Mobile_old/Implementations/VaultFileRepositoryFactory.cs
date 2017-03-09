using Erebus.Core.Contracts;
using Erebus.Core.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Implementations
{
    public class VaultFileRepositoryFactory : IVaultRepositoryFactory
    {
        private IFileSystem FileSystem;
        private ISymetricCryptographer SymetricCryptographer;
        private ISerializer Serializer;
        private IClockProvider ClockProvider;
        private IVaultFileMetadataHandler VaultFileMetadataHandler;


        public VaultFileRepositoryFactory(IFileSystem fileSystem, ISymetricCryptographer symetricCryptographer, ISerializer serializer, 
                                        IClockProvider clockProvider, IVaultFileMetadataHandler vaultFileMetadataHandler)
        {
            GuardClauses.ArgumentIsNotNull(nameof(fileSystem), fileSystem);
            GuardClauses.ArgumentIsNotNull(nameof(symetricCryptographer), symetricCryptographer);
            GuardClauses.ArgumentIsNotNull(nameof(serializer), serializer);
            GuardClauses.ArgumentIsNotNull(nameof(clockProvider), clockProvider);
            GuardClauses.ArgumentIsNotNull(nameof(vaultFileMetadataHandler), vaultFileMetadataHandler);

            this.FileSystem = fileSystem;
            this.SymetricCryptographer = symetricCryptographer;
            this.Serializer = serializer;
            this.ClockProvider = clockProvider;
            this.VaultFileMetadataHandler = vaultFileMetadataHandler;
        }

        public IVaultRepository CreateInstance()
        {
            string vaultsFolder = Constants.VAULT_FOLDER;

            return new VaultFileRepository(FileSystem, vaultsFolder, Constants.VAULT_FILE_NAME_EXTENSION,
                                                        SymetricCryptographer, Serializer, ClockProvider, VaultFileMetadataHandler);
        }
    }
}
