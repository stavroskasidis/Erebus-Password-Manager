using Erebus.Core.Contracts;
using Erebus.Core.Mobile.Contracts;
using Erebus.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Implementations
{
    public class Synchronizer : ISynchronizer
    {
        public event Action<string> StatusUpdate;

        private IServerCommunicator ServerCommunicator;
        private IVaultRepositoryFactory VaultRepositoryFactory;
        private IFileSystem FileSystem;
        private ISyncContext SyncContext;

        public Synchronizer(IServerCommunicator serverCommunicator, IVaultRepositoryFactory vaultRepositoryFactory, IFileSystem fileSystem,
                            ISyncContext syncContext)
        {
            this.ServerCommunicator = serverCommunicator;
            this.VaultRepositoryFactory = vaultRepositoryFactory;
            this.FileSystem = fileSystem;
            this.SyncContext = syncContext;
        }

        public async Task<bool> Synchronize()
        {
            this.SyncContext.Lock();
            try
            {
                bool canCommunicate = await this.ServerCommunicator.CanCommunicateWithServer();
                if (!canCommunicate)
                {
                    this.StatusUpdate?.Invoke(StringResources.ServerNotFound);
                    return false;
                }

                if (this.FileSystem.DirectoryExists(Constants.VAULT_FOLDER) == false)
                {
                    this.FileSystem.CreateDirectory(Constants.VAULT_FOLDER);
                }


                var serverVaultsInfo = await ServerCommunicator.GetVaultsInfoAsync();
                var vaultRepository = this.VaultRepositoryFactory.CreateInstance();
                var vaultsToDownload = new List<string>();
                var vaultsToDelete = new List<string>();

                //Delete
                foreach (string localVault in vaultRepository.GetAllVaultNames())
                {
                    bool existsOnServer = serverVaultsInfo.Any(x => x.VaultName == localVault);
                    if (!existsOnServer)
                    {
                        vaultsToDelete.Add(localVault);
                    }
                }

                foreach (var vaultToDelete in vaultsToDelete)
                {
                    FileSystem.DeleteFile(Path.ChangeExtension(Path.Combine(Constants.VAULT_FOLDER, vaultToDelete), Constants.VAULT_FILE_NAME_EXTENSION));
                }


                //Update
                foreach (var vaultInfo in serverVaultsInfo)
                {
                    bool exists = vaultRepository.VaultExists(vaultInfo.VaultName);
                    if (exists)
                    {
                        var existingVaultMetadata = vaultRepository.GetVaultMetadata(vaultInfo.VaultName);
                        if (vaultInfo.Version > existingVaultMetadata.Version)
                        {
                            vaultsToDownload.Add(vaultInfo.VaultName);
                        }
                    }
                    else
                    {
                        vaultsToDownload.Add(vaultInfo.VaultName);
                    }
                }

                foreach (string vaultName in vaultsToDownload)
                {
                    var vaultBytes = await ServerCommunicator.DownloadVaultAsync(vaultName);
                    this.FileSystem.WriteAllBytes(Path.ChangeExtension(Path.Combine(Constants.VAULT_FOLDER, vaultName), Constants.VAULT_FILE_NAME_EXTENSION), vaultBytes);
                }


                this.StatusUpdate?.Invoke(StringResources.Synchronized);
                return true;
            }
            finally
            {
                this.SyncContext.Release();
            }
        }
    }
}
