using DotNetCore.CAP;
using ECommerce.Order.Worker.Domain.Entities.Enums;
using ECommerce.Order.Worker.Domain.Services.Contracts;
using ECommerce.Order.Worker.Infra;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Order.Worker.Domain.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly OrderDbContext _orderDbContext;
    private readonly ICapPublisher _capPublisher;

    public PurchaseOrderService(OrderDbContext orderDbContext,
        ICapPublisher capPublisher)
    {
        _orderDbContext = orderDbContext;
        _capPublisher = capPublisher;
    }

    public async Task ChangeStatusAsync(Guid id, OrderStatus orderStatus, string gatewayName, Guid tranzactionId)
    {
        var order = await _orderDbContext
            .PurchaseOrders
            .Include(x => x.PurchaseOrderItems)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (order != null)
        {
            order.OrderStatus = orderStatus;
            order.GatewayName = gatewayName;
            order.TranzactionId = tranzactionId;
            order.UpdatedAt = DateTime.Now;
        }

        _orderDbContext.PurchaseOrders.Update(order);
        await _orderDbContext.SaveChangesAsync();

        if (order.OrderStatus == OrderStatus.Acepted)
            await _capPublisher.PublishAsync("ecomerce.catalog.stock", order.PurchaseOrderItems.Select(x =>
                new
                {
                    ItemId = x.ProductId,
                    x.Quantity
                }).ToArray());
    }
}
