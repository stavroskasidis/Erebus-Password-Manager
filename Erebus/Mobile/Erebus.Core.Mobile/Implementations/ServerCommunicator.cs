using Erebus.Core.Mobile.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erebus.Model.Service;
using System.Net.Http;
using Erebus.Core.Contracts;

namespace Erebus.Core.Mobile.Implementations
{
    public class ServerCommunicator : IServerCommunicator
    {
        private string ServerUrl;
        private ISerializer Serializer;

        public ServerCommunicator(string serverUrl, ISerializer serializer)
        {
            this.ServerUrl = serverUrl;
            this.Serializer = serializer;
        }


        public async Task<bool> CanCommunicateWithServer()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var methodUrl = new Uri(new Uri(ServerUrl), "MobileService/CommunicationCheck");
                    var response = await client.GetAsync(methodUrl, HttpCompletionOption.ResponseHeadersRead);
                    using (response)
                    {
                        return response.IsSuccessStatusCode;
                    }
                }
                catch (HttpRequestException)
                {
                    return false;
                }
            }
        }

        public async Task<byte[]> DownloadVaultAsync(string vaultName)
        {
            using (var client = new HttpClient())
            {
                var methodUrl = new Uri(new Uri(ServerUrl), "MobileService/DownloadVault?vaultName=" + vaultName);
                var response = await client.GetAsync(methodUrl);
                using (response)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
        }

        public async Task<List<VaultInfo>> GetVaultsInfoAsync()
        {
            using (var client = new HttpClient())
            {
                var methodUrl = new Uri(new Uri(ServerUrl), "MobileService/GetVaultsInfo");
                var response = await client.GetAsync(methodUrl);
                using (response)
                using (var content = response.Content)
                {
                    var result = await content.ReadAsStringAsync();
                    return Serializer.Deserialize<List<VaultInfo>>(result);
                }
            }
        }
    }
}
