using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiDemo.Api.Configuration;
using ApiDemo.Api.Services;
using Microsoft.Extensions.Options;

namespace ApiDemo.Tests.ServicesTests;

sealed public class TokenServiceTests
{
    private readonly TokenService _service;

    public TokenServiceTests()
    {
        var jwtSettings = new JwtSettings
        {
            Key = "this-is-a-test-key-with-enough-length-123456",
            Issuer = "test-issuer",
            Audience = "test-audience",
            ExpiresHours = 1
        };

        _service = new TokenService(Options.Create(jwtSettings));
    }

    [Fact]
    public void CreateToken_WithAdministratorRole_ReturnsValidJwt()
    {
        var token = _service.CreateToken("admin", "Administrator");

        Assert.False(string.IsNullOrWhiteSpace(token));

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.Equal("test-issuer", jwt.Issuer);
        Assert.Contains("test-audience", jwt.Audiences);
        Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "admin");
        Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Administrator");
        Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Role && c.Value == "User");
    }

    [Fact]
    public void CreateToken_WithUserRole_ReturnsValidJwt()
    {
        var token = _service.CreateToken("user", "User");

        Assert.False(string.IsNullOrWhiteSpace(token));

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.Equal("test-issuer", jwt.Issuer);
        Assert.Contains("test-audience", jwt.Audiences);
        Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Name && c.Value == "user");
        Assert.DoesNotContain(jwt.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Administrator");
        Assert.Contains(jwt.Claims, c => c.Type == ClaimTypes.Role && c.Value == "User");
    }
}