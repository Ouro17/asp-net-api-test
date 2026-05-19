using System.Text;
using ApiDemo.Api.Configuration;
using ApiDemo.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSection);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var jwtSettings = jwtSection.Get<JwtSettings>()!;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services
    .AddAuthorizationBuilder()
    .AddPolicy("AdminsPolicy", policy => policy.RequireRole("Administrator"))
    .AddPolicy("UsersPolicy", policy => policy.RequireRole("User", "Administrator"));

builder.Services.TryAddSingleton<IProductService, ProductService>();
builder.Services.TryAddSingleton<ITokenService, TokenService>();
builder.Services.TryAddSingleton<ILoginService, LoginService>();

var app = builder.Build();

app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthentication(); // Actually log the user
app.UseAuthorization(); // Only enforces access rules

app.MapControllers();

app.Run();
