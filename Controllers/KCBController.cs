using BC_Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BC_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KcbController : Controller
    {
        private readonly KCBService _kcbService;

        public KcbController(KCBService kcbService)
        {
            _kcbService = kcbService;
        }

        [HttpGet("token")]
        public async Task<IActionResult> GetToken()
        {
            // Call the service to fetch the token
            var (accessToken, tokenType, expiresIn) = await _kcbService.FetchTokenAsync();

            // Return the token details in JSON format
            return Ok(new
            {
                access_token = accessToken,
                token_type = tokenType,
                expires_in = expiresIn
            });
        }
    }
}
