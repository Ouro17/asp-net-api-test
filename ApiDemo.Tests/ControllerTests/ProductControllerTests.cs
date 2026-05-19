using ApiDemo.Api.Controllers;
using ApiDemo.Api.Domain;
using ApiDemo.Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace ApiDemo.Tests.ControllerTests;

sealed public class ProductsControllerTests
{
    private readonly Mock<IProductService> _productService;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _productService = new Mock<IProductService>();
        _controller = new ProductsController(_productService.Object);
    }

    [Fact]
    public void Get_ReturnsOkWithProducts()
    {
        _productService
            .Setup(x => x.GetAll())
            .Returns(["Keyboard"]);

        var result = _controller.GetAll();

        var ok = Assert.IsType<Ok<GetAllProductsResponse>>(result);
        Assert.Contains("Keyboard", ok.Value?.Products!);

        _productService.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public void Add_CorrectData_ReturnsCreated()
    {
        _productService
            .Setup(x => x.Add(It.IsAny<string>()))
            .Returns(true);

        var result = _controller.Add(new CreateProductRequest("Test"));

        Assert.IsType<Results<Created, BadRequest>>(result);
        Assert.IsType<Created>(result.Result);

        _productService.Verify(c => c.Add("Test"), Times.Once);
    }

    [Fact]
    public void Add_IncorrectData_ReturnsBadRequest()
    {
        _productService
            .Setup(x => x.Add(It.IsAny<string>()))
            .Returns(false);

        var result = _controller.Add(new CreateProductRequest("Test"));

        Assert.IsType<Results<Created, BadRequest>>(result);
        Assert.IsType<BadRequest>(result.Result);

        _productService.Verify(c => c.Add("Test"), Times.Once);
    }
}