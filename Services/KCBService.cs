using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BC_Api.Services
{
    public class KCBService
    {
        private readonly HttpClient _httpClient;

        public KCBService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(string AccessToken, string TokenType, int ExpiresIn)> FetchTokenAsync()
        {
            var url = "https://uat.buni.kcbgroup.com/token?grant_type=client_credentials";

            // Your Base64-encoded credentials
            var encodedCredentials = "YWVPRV90eWxYT2themFFZTdWTHRhTkk2bnhvYTpfTUR4OGtBc1h5aHRUN0VreWoxdmxaZnNMRVlh";

            // Set the Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);

            // Send POST request
            var response = await _httpClient.PostAsync(url, null);
            response.EnsureSuccessStatusCode(); // Throws an exception if the response is not successful

            // Parse the JSON response
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(jsonResponse);
            var accessToken = jsonDocument.RootElement.GetProperty("access_token").GetString();
            var tokenType = jsonDocument.RootElement.GetProperty("token_type").GetString();
            var expiresIn = jsonDocument.RootElement.GetProperty("expires_in").GetInt32();

            return (accessToken!, tokenType!, expiresIn);
        }
    }
}
