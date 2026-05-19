using ApiDemo.Api.Services;

namespace ApiDemo.Tests.ServicesTests;

sealed public class LoginServiceTests
{
    private readonly LoginService _service;

    public LoginServiceTests() => _service = new LoginService();

    [Theory]
    [InlineData("admin", "1234", true)]
    [InlineData("user", "4321", true)]
    [InlineData("admin", "4321", false)]
    [InlineData("user", "1234", false)]
    [InlineData("unknown", "1234", false)]
    [InlineData("Admin", "1234", false)] // Case sensitive
    [InlineData("", "", false)]
    public void ValidateCredentials_ReturnsExpectedResult(
        string username,
        string password,
        bool expected)
    {
        var result = _service.ValidateCredentials(username, password);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("admin", "Administrator")]
    [InlineData("user", "User")]
    [InlineData("something-else", "Undefined")]
    [InlineData("", "Undefined")]
    public void GetRole_ReturnsExpectedRole(
        string username,
        string expectedRole)
    {
        var result = _service.GetRole(username);

        Assert.Equal(expectedRole, result);
    }
}