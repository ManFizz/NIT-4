using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToDo_RestAPI.Auth;
using ToDo_RestAPI.Data;
// условная бд с пользователями
var people = new List<Person>
{
    new Person("tom@gmail.com", "12345"),
    new Person("bob@gmail.com", "55555")
};

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddAuthorization();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,
            
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });

services.AddControllersWithViews();

services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.OAuth2,
        Scheme = "Bearer"
    });
});

services.AddDbContext<TodoDbContext>(options =>
{
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", (Person loginData) => 
{
    // находим пользователя 
    var person = people.FirstOrDefault(p => p.Email == loginData.Email && p.Password == loginData.Password);
    // если пользователь не найден, отправляем статусный код 401
    if(person is null) return Results.Unauthorized();
     
    var claims = new List<Claim> {new(ClaimTypes.Name, person.Email) };
    // создаем JWT-токен
    var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
 
    // формируем ответ
    var response = new
    {
        access_token = encodedJwt,
        username = person.Email
    };
 
    return Results.Json(response);
});
app.UseAuthorization();
app.Map("/data", [Authorize] () => new { message= "Hello World!" });

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

internal record class Person(string Email, string Password);