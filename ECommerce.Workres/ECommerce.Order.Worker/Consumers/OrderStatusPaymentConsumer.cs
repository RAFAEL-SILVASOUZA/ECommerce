using DotNetCore.CAP;
using ECommerce.Order.Worker.Domain.Entities.Enums;
using ECommerce.Order.Worker.Domain.Services.Contracts;
using ECommerce.Order.Worker.Models.Response;

namespace ECommerce.Order.Worker.Consumer;

public interface IOrderStatusOrderStatusPaymentConsumer
{
    Task ProccessMessageAsync(PurchaseOrderPaymentResponse purchaseOrderPaymentResponse);
}

public class OrderStatusOrderStatusPaymentConsumer : IOrderStatusOrderStatusPaymentConsumer, ICapSubscribe
{
    private readonly IPurchaseOrderService _purchaseOrderService;

    public OrderStatusOrderStatusPaymentConsumer(IPurchaseOrderService purchaseOrderService)
    {
        _purchaseOrderService = purchaseOrderService;
    }

    [CapSubscribe("ecomerce.order.status.payment")]
    public async Task ProccessMessageAsync(PurchaseOrderPaymentResponse purchaseOrderPaymentResponse)
    {
        var orderStatus = purchaseOrderPaymentResponse.PaymentStatus == PaymentStatus.Accepted
            ? OrderStatus.Acepted
            : OrderStatus.Rejected;

        await _purchaseOrderService.ChangeStatusAsync(purchaseOrderPaymentResponse.OrderId, orderStatus,
                   purchaseOrderPaymentResponse.GatewayName, purchaseOrderPaymentResponse.TranzactionId);
    }
}
