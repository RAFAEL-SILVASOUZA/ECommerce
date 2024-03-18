using ECommerce.Order.Worker.Domain.Entities.Enums;

namespace ECommerce.Order.Worker.Domain.Services.Contracts;

public interface IPurchaseOrderService
{
    Task ChangeStatusAsync(Guid id, OrderStatus orderStatus, string gatewayName, Guid tranzactionId);
}
