namespace ApiDemo.Api.Services;

sealed public class ProductService : IProductService
{
    private readonly List<string> _products =
    [
        "Keyboard", "Mouse", "Monitor"
    ];

    public IEnumerable<string> GetAll() => _products;

    public bool Add(string product)
    {
        if (string.IsNullOrWhiteSpace(product)
            || _products.Contains(product))
        {
            return false;
        }

        _products.Add(product);

        return true;
    }
}