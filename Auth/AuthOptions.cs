using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ToDo_RestAPI.Auth;

public class AuthOptions : JwtBearerOptions
{
    public const string DefaultScheme = "MyAuthenticationScheme";
    public string TokenHeaderName { get; set; } = "Bearer";
}