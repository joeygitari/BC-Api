using Microsoft.Extensions.Caching.Memory;
using BC_Api.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BC_Api.Services
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly string _hostUrl = "https://uat.buni.kcbgroup.com";
        private readonly string _credentials = "YWVPRV90eWxYT2themFFZTdWTHRhTkk2bnhvYTpfTUR4OGtBc1h5aHRUN0VreWoxdmxaZnNMRVlh";

        public TokenService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
            _httpClient.BaseAddress = new Uri(_hostUrl);
        }

        public async Task<string> GetTokenAsync()
        {
            // Try to retrieve token from cache
            if (_cache.TryGetValue("AccessToken", out string cachedToken))
            {
                return cachedToken;
            }

            // Request new token if not in cache
            var request = new HttpRequestMessage(HttpMethod.Post, "/token?grant_type=client_credentials");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _credentials);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonResponse);

            // Cache the token with an expiration time (adjust as needed)
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            };
            _cache.Set("AccessToken", tokenResponse.AccessToken, cacheOptions);

            return tokenResponse.AccessToken;
        }
    }
}
