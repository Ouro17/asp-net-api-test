namespace ApiDemo.Api.Services;

public interface ITokenService
{
    string CreateToken(string username, string role);
}