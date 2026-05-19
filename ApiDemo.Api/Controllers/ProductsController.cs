using ApiDemo.Api.Domain;
using ApiDemo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
sealed public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [Authorize(Policy = "UsersPolicy"), HttpGet]
    public Ok<GetAllProductsResponse> GetAll()
        => TypedResults.Ok(new GetAllProductsResponse(_productService.GetAll()));

    [Authorize(Policy = "AdminsPolicy"), HttpPost]
    public Results<Created, BadRequest> Add(CreateProductRequest data)
    {
        if (_productService.Add(data.Product))
        {
            return TypedResults.Created();
        }

        return TypedResults.BadRequest();
    }
}