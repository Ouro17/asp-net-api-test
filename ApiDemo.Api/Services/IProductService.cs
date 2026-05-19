namespace ApiDemo.Api.Services;

public interface IProductService
{
    IEnumerable<string> GetAll();

    bool Add(string product);
}