using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Newtonsoft.Json;
#pragma warning disable CS0618 // Type or member is obsolete

namespace ToDo_RestAPI.Auth;

public class AuthHandler : AuthenticationHandler<AuthOptions>
{
    private static ILogger _logger = null!;
    public AuthHandler(IOptionsMonitor<AuthOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _logger = logger.CreateLogger<Program>();
    }
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var tokens))
            return AuthenticateResult.Fail("Missing header: Authorization");
        var token = tokens.FirstOrDefault();
        
        if (string.IsNullOrEmpty(token) || !token.StartsWith(AuthOptions.TokenHeaderName) || token.Length <= 1000)
            return AuthenticateResult.Fail("Invalid token format");
        
        token = token.Substring(AuthOptions.TokenHeaderName.Length);
        
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        if (jsonToken == null)
            return AuthenticateResult.Fail("Invalid token");
        
        var userInfo = await IsValidKeyCloakToken(token);
        if(userInfo == null)
            return AuthenticateResult.Fail("Invalid token by keycloak.");
        
        var roles = jsonToken.Claims
            .Where(c => c.Type == "realm_access")
            .SelectMany(c => c.Value.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .ToList();

        if (roles.Count == 0)
        {
            return AuthenticateResult.Fail("No roles found in token");
        }
        
        var claims = new List<Claim>()
        {
            new("FirstName", userInfo.preferred_username),
        };

        foreach (var role in roles) 
            claims.Add(new Claim(ClaimTypes.Role, role));

        var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        
        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
    }
    
    private static readonly HttpClient Client = new();

    private static async Task<UserInfo?> IsValidKeyCloakToken(string token)
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await Client.PostAsync("http://localhost:8080/auth/realms/master/protocol/openid-connect/userinfo", null);
        var responseString = await response.Content.ReadAsStringAsync();
        _logger.LogDebug("Answer from keycloak" + responseString);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Error response");

            return null;
        }
        var userInfo = JsonConvert.DeserializeAnonymousType(responseString, new UserInfo());
        _logger.LogInformation("Successful response");
        return userInfo;
    }

    private class UserInfo
    {
        public string sub { get; set; }
        public bool email_verified { get; set; }
        public string preferred_username { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
    }
}