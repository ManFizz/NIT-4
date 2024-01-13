using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ToDo_RestAPI.Auth;

namespace ToDo_RestAPI.Controllers;

[method: JsonConstructor]
public record AuthData(string ClientId, string ClientSecret, string Username, string Password);

public class LoginController : Controller
{
    [AllowAnonymous]
    [HttpPost("/login")]
    public async Task<IActionResult> GetToken([FromBody] AuthData? authData)
    {
        if (authData == null)
        {
            authData = new AuthData(
                "vladimir",
                "sz6tQBqsGeBBcitjUhUFVqi7tYbcnrUK",
                "testuser",
                "Amogus77"
            );
        }

        var token = await GetTokenByPass(authData);
        
        return Ok(token);
    }
    
    private static readonly HttpClient Client = new();
    private static async Task<TokenResponse> GetTokenByPass(AuthData authData)
    {
        var values = new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", authData.ClientId },
            { "client_secret", authData.ClientSecret },
            { "username", authData.Username },
            { "password", authData.Password },
        };

        var content = new FormUrlEncodedContent(values);

        var response = await Client.PostAsync("http://localhost:8080/auth/realms/master/protocol/openid-connect/token", content);

        var responseString = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseString)!;

        Console.WriteLine(tokenResponse.access_token);
        return tokenResponse;
    }
}