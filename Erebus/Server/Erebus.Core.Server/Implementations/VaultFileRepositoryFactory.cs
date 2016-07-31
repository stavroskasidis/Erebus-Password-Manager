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
        private IServerConfigurationProvider ServerConfigurationProvider;
        private IClockProvider ClockProvider;

        public VaultFileRepositoryFactory(IFileSystem fileSystem, ISymetricCryptographer symetricCryptographer, ISerializer serializer, IServerConfigurationProvider serverConfigurationProvider,
                                          IClockProvider clockProvider)
        {
            GuardClauses.ArgumentIsNotNull(nameof(fileSystem), fileSystem);
            GuardClauses.ArgumentIsNotNull(nameof(symetricCryptographer), symetricCryptographer);
            GuardClauses.ArgumentIsNotNull(nameof(serializer), serializer);
            GuardClauses.ArgumentIsNotNull(nameof(serverConfigurationProvider), serverConfigurationProvider);
            GuardClauses.ArgumentIsNotNull(nameof(clockProvider), clockProvider);

            this.FileSystem = fileSystem;
            this.SymetricCryptographer = symetricCryptographer;
            this.Serializer = serializer;
            this.ServerConfigurationProvider = serverConfigurationProvider;
            this.ClockProvider = clockProvider;
        }

        public IVaultRepository CreateInstance()
        {
            return new VaultFileRepository(FileSystem, ServerConfigurationProvider.GetConfiguration().VaultsFolder, Constants.VAULT_FILE_NAME_EXTENSION, SymetricCryptographer, Serializer, ClockProvider);
        }
    }
}
