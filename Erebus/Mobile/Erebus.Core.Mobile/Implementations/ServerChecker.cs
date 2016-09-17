using Erebus.Core.Mobile.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Core.Mobile.Implementations
{
    public class ServerChecker : IServerChecker
    {
        public async Task<bool> IsServerOnlineAsync(string serverUrl)
        {
            using (var client = new HttpClient())
            {
                var message = await client.GetAsync(serverUrl, HttpCompletionOption.ResponseHeadersRead);
                return message.IsSuccessStatusCode;
            }
        }
    }
}
