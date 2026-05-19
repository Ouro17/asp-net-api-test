using ApiDemo.Api.Domain;
using ApiDemo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
sealed public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [Authorize(Policy = "UsersPolicy"), HttpGet]
    public ActionResult<IEnumerable<string>> Get() => Ok(_productService.GetAll());

    [Authorize(Policy = "AdminsPolicy"), HttpPost]
    public ActionResult Add(CreateProductRequest data)
    {
        if (_productService.Add(data.Product))
        {
            return Created();
        }

        return BadRequest(User.Identity?.Name);
    }
}