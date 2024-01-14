using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ToDo_RestAPI.Auth;

public class AuthOptions : JwtBearerOptions
{
    public const string DefaultScheme = "MyAuthenticationScheme";
    public const string TokenHeaderName = "Bearer ";
}