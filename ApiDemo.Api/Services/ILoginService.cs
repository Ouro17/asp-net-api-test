namespace ApiDemo.Api.Services;

public interface ILoginService
{
    bool ValidateCredentials(string username, string password);
    string GetRole(string username);
}