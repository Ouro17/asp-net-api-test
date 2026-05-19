using ApiDemo.Api.Domain;
using ApiDemo.Api.Services;
using Microsoft.AspNetCore.Authorization;
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
    public ActionResult<string> Role(GetRoleRequest data) => Ok(_loginService.GetRole(data.Username));

    [AllowAnonymous, HttpPost]
    public IResult Login(LoginRequest request)
    {
      if (!_loginService.ValidateCredentials(request.Username, request.Password))
        {
            return Results.Unauthorized();
        }

        var role = _loginService.GetRole(request.Username);
        var token = _tokenService.CreateToken(request.Username, role);

        return Results.Ok(new { token });
    }
}