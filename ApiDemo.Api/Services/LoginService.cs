namespace ApiDemo.Api.Services;

sealed public class LoginService : ILoginService
{
    public bool ValidateCredentials(string username, string password)
    {
        if (username == "admin" && password == "1234")
        {
            return true;
        }

        if (username == "user" && password == "4321")
        {
            return true;
        }

        return false;
    }

    public string GetRole(string username) => username switch
    {
        "admin" => "Administrator",
        "user" => "User",
        _ => "Undefined"
    };
}