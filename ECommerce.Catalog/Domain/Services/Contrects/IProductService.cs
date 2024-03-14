using ECommerce.Catalog.Domain.Entities;

namespace ECommerce.Catalog.Domain.Services.Contrects;

public interface IProductService
{
    Task ProccessMessageAsync(ProductStock[] productStock);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<IEnumerable<Product>> GetProductsByIdsAsync(Guid[] ids);

    Task<Product> ChangeQuantityByProductIdAsync(Guid id, int quantity);
}
