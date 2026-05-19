using ApiDemo.Api.Services;

namespace ApiDemo.Tests.ServicesTests;

sealed public class ProductServiceTests
{
    private readonly ProductService _service;

    public ProductServiceTests() => _service = new ProductService();

    [Fact]
    public void GetAll_ReturnsDefaultProducts()
    {
        var result = _service.GetAll().ToList();

        Assert.Contains("Keyboard", result);
        Assert.Contains("Mouse", result);
        Assert.Contains("Monitor", result);
    }

    [Fact]
    public void Add_WithValidNewProduct_ReturnsTrue()
    {
        var productName = Guid.NewGuid().ToString();

        var result = _service.Add(productName);

        Assert.True(result);
        Assert.Contains(productName, _service.GetAll());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Add_WithEmptyOrWhitespace_ReturnsFalse(string product)
    {
        var result = _service.Add(product);

        Assert.False(result);
    }

    [Fact]
    public void Add_WithDuplicateProduct_ReturnsFalse()
    {
        var result = _service.Add("Keyboard");

        Assert.False(result);
    }
}