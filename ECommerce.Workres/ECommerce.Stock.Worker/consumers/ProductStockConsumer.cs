using DotNetCore.CAP;
using ECommerce.Stock.Worker.Entities;
using ECommerce.Stock.Worker.Infra;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Stock.Worker.Consumers
{
    public interface IProductStockConsumer { }

    public class ProductStockConsumer : IProductStockConsumer, ICapSubscribe
    {
        private readonly ProductDBContext _productDBContext;

        public ProductStockConsumer(ProductDBContext productDBContext)
           => _productDBContext = productDBContext;

        [CapSubscribe("ecomerce.catalog.stock")]
        public async Task ProccessMessageAsync(ProductStock[] productsStock)
        {
            var ids = productsStock.Select(p => p.ItemId).ToArray();
            var products = await _productDBContext
                .Products
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            foreach (var product in products)
            {
                var stockProduct = productsStock.Single(x => x.ItemId == product.Id);
                product.Quantity -= stockProduct.Quantity;
            }

            await _productDBContext.SaveChangesAsync();
        }
    }
}
