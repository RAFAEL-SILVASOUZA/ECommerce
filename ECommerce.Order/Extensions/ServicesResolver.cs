using ECommerce.Order.Domain.Services;
using ECommerce.Order.Domain.Services.Contracts;
using ECommerce.Order.Infra;

namespace ECommerce.Order.Extensions
{
    public static class ServicesResolver
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<ICatalogItemService, CatalogItemService>();
            services.AddTransient<IPurchaseOrderService, PurchaseOrderService>();
            services.AddDbContext<OrderDbContext>();
        }
    }
}
