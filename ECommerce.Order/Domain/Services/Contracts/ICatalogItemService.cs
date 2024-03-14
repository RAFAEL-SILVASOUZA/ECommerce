using ECommerce.Order.Domain.Entities;

namespace ECommerce.Order.Domain.Services.Contracts
{
    public interface ICatalogItemService
    {
        Task<IList<CatalogItem>> GetProductsByIdsAsync(Guid[] ids);
    }
}
