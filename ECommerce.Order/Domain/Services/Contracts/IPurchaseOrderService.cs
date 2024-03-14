using ECommerce.Order.Domain.Entities.Enums;
using ECommerce.Order.Models.Request;
using ECommerce.Order.Models.Response;

namespace ECommerce.Order.Domain.Services.Contracts
{
    public interface IPurchaseOrderService
    {
        Task<IList<PurchaseOrderResponse>> GetAllOrdersAsync();
        Task<PurchaseOrderResponse> GetOrderByIdAsync(Guid id);
        Task<PurchaseOrderResponse> CreateOrderAsync(PurchaseOrderCreateRequest purchaseOrderCreateRequest);
        Task<PurchaseOrderResponse> ReproccessOrderAsync(PurchaseOrderReproccessRequest purchaseOrderReproccessRequest);
        Task ChangeStatusAsync(Guid id, OrderStatus orderStatus, string gatewayName, Guid tranzactionId);
    }
}
