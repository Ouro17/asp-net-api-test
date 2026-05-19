using ApiDemo.Api.Domain;
using ApiDemo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
sealed public class LoginController(
    ILoginService loginService,
    ITokenService tokenService) : ControllerBase
{
    private readonly ILoginService _loginService = loginService;
    private readonly ITokenService _tokenService = tokenService;

    [Authorize(Policy = "AdminsPolicy"), HttpGet("role")]
    public Ok<RoleResponse> Role(GetRoleRequest data) => TypedResults.Ok(new RoleResponse(_loginService.GetRole(data.Username)));

    [AllowAnonymous, HttpPost]
    public Results<UnauthorizedHttpResult, Ok<LoginResponse>> Login(LoginRequest request)
    {
        if (!_loginService.ValidateCredentials(request.Username, request.Password))
        {
            return TypedResults.Unauthorized();
        }

        var role = _loginService.GetRole(request.Username);
        var token = _tokenService.CreateToken(request.Username, role);

        return TypedResults.Ok(new LoginResponse(token));
    }
}