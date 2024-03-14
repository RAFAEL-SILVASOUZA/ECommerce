using DotNetCore.CAP;
using ECommerce.Order.Domain.Entities.Enums;
using ECommerce.Order.Domain.Services.Contracts;
using ECommerce.Order.Models.Response;

namespace ECommerce.Order.Domain.Consumer
{
    public class OrderStatusOrderStatusPaymentConsumer : IOrderStatusPaymentConsumer, ICapSubscribe
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public OrderStatusOrderStatusPaymentConsumer(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        [CapSubscribe("ecomerce.order.status.payment")]
        public async Task ProccessMessage(PurchaseOrderPaymentResponse purchaseOrderPaymentResponse)
        {
            var orderStatus = purchaseOrderPaymentResponse.PaymentStatus == PaymentStatus.Accepted
                ? OrderStatus.Acepted
                : OrderStatus.Rejected;

            await _purchaseOrderService.ChangeStatusAsync(purchaseOrderPaymentResponse.OrderId, orderStatus,
                       purchaseOrderPaymentResponse.GatewayName, purchaseOrderPaymentResponse.TranzactionId);
        }
    }
}
