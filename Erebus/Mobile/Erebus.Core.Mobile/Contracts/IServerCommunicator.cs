using Erebus.Model.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Contracts
{
    public interface IServerCommunicator
    {
        Task<bool> CanCommunicateWithServer();
        Task<List<VaultInfo>> GetVaultsInfoAsync();
        Task<byte[]> DownloadVaultAsync(string vaultName);
    }
}
