using ApiDemo.Api.Controllers;
using ApiDemo.Api.Domain;
using ApiDemo.Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace ApiDemo.Tests.ControllerTests;

sealed public class LoginControllerTests
{
    private readonly Mock<ILoginService> _loginService;
    private readonly Mock<ITokenService> _tokenService;
    private readonly LoginController _controller;

    public LoginControllerTests()
    {
        _loginService = new Mock<ILoginService>();
        _tokenService = new Mock<ITokenService>();
        _controller = new LoginController(_loginService.Object, _tokenService.Object);
    }

    [Fact]
    public void Login_WithValidCredentials_ReturnsOkWithToken()
    {
        _loginService
            .Setup(x => x.ValidateCredentials("admin", "1234"))
            .Returns(true);

        _loginService
            .Setup(x => x.GetRole("admin"))
            .Returns("Administrator");

        _tokenService
            .Setup(x => x.CreateToken("admin", "Administrator"))
            .Returns("fake-jwt-token");

        var result = _controller.Login(new LoginRequest("admin", "1234"));

        Assert.IsType<Results<UnauthorizedHttpResult, Ok<LoginResponse>>>(result);
        var ok = Assert.IsType<Ok<LoginResponse>>(result.Result);
        Assert.Equal("fake-jwt-token", ok.Value?.Token);

        _loginService.Verify(c => c.ValidateCredentials("admin", "1234"), Times.Once);
        _loginService.Verify(c => c.GetRole("admin"), Times.Once);
        _tokenService.Verify(c => c.CreateToken("admin", "Administrator"), Times.Once);
    }

    [Fact]
    public void Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        _loginService
            .Setup(x => x.ValidateCredentials("admin", "bad"))
            .Returns(false);

        var result = _controller.Login(new LoginRequest("admin", "bad"));

        Assert.IsType<Results<UnauthorizedHttpResult, Ok<LoginResponse>>>(result);
        Assert.IsType<UnauthorizedHttpResult>(result.Result);

        _loginService.Verify(c => c.ValidateCredentials("admin", "bad"), Times.Once);
    }

    [Fact]
    public void Role_ReturnsOk()
    {
        _loginService
            .Setup(x => x.GetRole("admin"))
            .Returns("Administrator");

        var result = _controller.Role(new GetRoleRequest("admin"));

        var ok = Assert.IsType<Ok<RoleResponse>>(result);
        Assert.Equal("Administrator", ok.Value?.Role);

        _loginService.Verify(x => x.GetRole("admin"), Times.Once);
    }
}