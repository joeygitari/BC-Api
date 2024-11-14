using System.Security.Cryptography;
using System.Text;

namespace BC_Api.Token
{
    public static class TokenHelper
    {
        private static readonly string preSharedKey = "Jo7ygf789.Token";

        // Generate a token using the pre-shared key
        public static string GenerateToken()
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(preSharedKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(preSharedKey));
                return Convert.ToBase64String(hash);
            }
        }

        // Validate the provided token against the expected token
        public static bool ValidateToken(string token)
        {
            var expectedToken = GenerateToken();
            return token == expectedToken;
        }
    }
}