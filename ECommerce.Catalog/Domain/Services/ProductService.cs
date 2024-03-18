using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.Services.Contrects;
using ECommerce.Catalog.Infra;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Domain.Consumer;

public class ProductService : IProductService
{
    private readonly CatalogDBContext _catalogDbContext;

    public ProductService(CatalogDBContext catalogDbContext)
       => _catalogDbContext = catalogDbContext;

    public async Task<IEnumerable<Product>> GetProductsAsync()
       => await _catalogDbContext.Products.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Product>> GetProductsByIdsAsync(Guid[] ids)
       => await _catalogDbContext.Products
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking()
            .ToListAsync();

    public async Task<Product> ChangeQuantityByProductIdAsync(Guid id, int quantity)
    {
        var product = await _catalogDbContext.Products.FindAsync(id);

        if (product is null)
            throw new ArgumentNullException("Product not found");

        product.Quantity += quantity;

        await _catalogDbContext.SaveChangesAsync();

        return product;
    }
}
