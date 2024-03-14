using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.Services.Contrects;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
       => _productService = productService;

    [HttpGet]
    public async Task<IEnumerable<Product>> Get()
       => await _productService.GetProductsAsync();

    [HttpGet("{ids}")]
    public async Task<IEnumerable<Product>> Get([FromQuery(Name = "ids")] Guid[] ids)
       => await _productService.GetProductsByIdsAsync(ids);


    [HttpPatch("{id}")]
    public async Task<Product> Patch([FromQuery(Name = "id")] Guid id, [FromBody] int quantity)
    => await _productService.ChangeQuantityByProductIdAsync(id, quantity);
}
