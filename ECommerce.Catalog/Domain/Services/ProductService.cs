using DotNetCore.CAP;
using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.Services.Contrects;
using ECommerce.Catalog.Infra;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Domain.Consumer;

public class ProductService : IProductService, ICapSubscribe
{
    private readonly CatalogDBContext _catalogDbContext;

    public ProductService(CatalogDBContext catalogDbContext)
       => _catalogDbContext = catalogDbContext;

    [CapSubscribe("ecomerce.catalog.stock")]
    public async Task ProccessMessageAsync(ProductStock[] productsStock)
    {
        var ids = productsStock.Select(p => p.ItemId).ToArray();
        var products = await _catalogDbContext
            .Products
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();

        foreach (var product in products)
        {
            var stockProduct = productsStock.Single(x => x.ItemId == product.Id);
            product.Quantity -= stockProduct.Quantity;
        }

        await _catalogDbContext.SaveChangesAsync();
    }

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
