using PersonalFinanceTracker.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Services.Infrastructure
{
    public class NetworkService : INetworkService
    {
        private HttpClient _client;

        public NetworkService(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> IsServerReachableAsync()
        {
            try
            {
                var response = await _client.GetAsync("health");
                return response.IsSuccessStatusCode;

            }
            catch (HttpRequestException ex)
            {
                // Logging
                return false;
            }
            catch (TaskCanceledException ex)
            {
                // Logging
                return false;
            }
            catch (Exception ex)
            {
                // Logging
                return false;
            }


        }
    }
}
