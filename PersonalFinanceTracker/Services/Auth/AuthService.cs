using PersonalFinanceTracker.Helpers.Logic;
using PersonalFinanceTracker.Interfaces.Auth;
using PersonalFinanceTracker.Interfaces.Infrastructure;
using PersonalFinanceTracker.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Services.Auth
{
    public class AuthService : IAuthService
    {

        private readonly HttpClient _client;
        private readonly IUserSessionService _userSessionService;
        private readonly INetworkService _networkService;

        public AuthService(IUserSessionService userSessionService, INetworkService networkService, HttpClient client)
        {
            _userSessionService = userSessionService;
            _networkService = networkService;
            _client = client;

        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            if (!await _networkService.IsServerReachableAsync())
            {
                return false;
            }

            UserDto user = new UserDto(username, password);
            var response = await _client.PostAsJsonAsync("api/auth/login", user);

            if (!response.IsSuccessStatusCode)
            {
                // Error Log
                return false;
            }


            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            _userSessionService.SetSession(username, result?.Token);

            return result != null;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            if (!await _networkService.IsServerReachableAsync())
                return false;

            UserDto user = new UserDto(username, password);
            var response = await _client.PostAsJsonAsync("api/auth/register", user);
            if (!response.IsSuccessStatusCode)
            {
                // Error Log
                return false;
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> IsUserRegisteredAsync(string username)
        {
            if (!await _networkService.IsServerReachableAsync())
                return false;

            var response = await _client.PostAsJsonAsync("api/auth/check-username", username);
            if (!response.IsSuccessStatusCode)
            {
                // Error Log
                return false;
            }

            return response.IsSuccessStatusCode;

        }

    }
}
