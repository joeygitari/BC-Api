using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using BC_Api.Services;

public class TokenAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenService _tokenService;

    public TokenAuthenticationMiddleware(RequestDelegate next, TokenService tokenService)
    {
        _next = next;
        _tokenService = tokenService;
    }

    public async Task Invoke(HttpContext context)
    {
        // Retrieve token from the TokenService
        var token = await _tokenService.GetTokenAsync();

        // Add token to request headers
        context.Request.Headers["Authorization"] = $"Bearer {token}";

        await _next(context);
    }
}
