using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ToDo_RestAPI.Auth;

public class AuthHandler(
    IOptionsMonitor<AuthOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock)
    : AuthenticationHandler<AuthOptions>(options, logger, encoder, clock)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        const string tokenHeaderName = "Bearer";
        
        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            return AuthenticateResult.Fail($"Missing header: Authorization");
        }

        if(token!= tokenHeaderName + " " + "supersecretecode")
        {
            return AuthenticateResult
                .Fail($"Invalid token.");
        }
        
        var claims = new List<Claim>()
        {
            new("FirstName", "Juan")
        };

        var claimsIdentity = new ClaimsIdentity
            (claims, Scheme.Name);
        var claimsPrincipal = new ClaimsPrincipal 
            (claimsIdentity);

        return AuthenticateResult.Success
        (new AuthenticationTicket(claimsPrincipal, 
            Scheme.Name));
    }
}