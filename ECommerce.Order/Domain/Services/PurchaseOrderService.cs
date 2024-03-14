using DotNetCore.CAP;
using ECommerce.Order.Domain.Entities;
using ECommerce.Order.Domain.Entities.Enums;
using ECommerce.Order.Domain.Messages;
using ECommerce.Order.Domain.Services.Contracts;
using ECommerce.Order.Infra;
using ECommerce.Order.Models.Request;
using ECommerce.Order.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Order.Domain.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly OrderDbContext _orderDbContext;
    private readonly ICatalogItemService _catalogItemService;
    private readonly ICapPublisher _capPublisher;

    public PurchaseOrderService(OrderDbContext orderDbContext, 
        ICatalogItemService catalogItemService, 
        ICapPublisher capPublisher)
    {
        _orderDbContext = orderDbContext;
        _catalogItemService = catalogItemService;
        _capPublisher = capPublisher;
    }

    public async Task<IList<PurchaseOrderResponse>> GetAllOrdersAsync()
    {
        var orders = await _orderDbContext.PurchaseOrders
            .Include(X => X.PurchaseOrderItems)
            .AsNoTracking()
            .ToListAsync();
        return orders.Select(x => new PurchaseOrderResponse(x)).ToList();
    }

    public async Task<PurchaseOrderResponse> GetOrderByIdAsync(Guid id)
    {
        var order = await _orderDbContext.PurchaseOrders
            .Include(X => X.PurchaseOrderItems)
            .FirstOrDefaultAsync(X => X.Id == id);
        return order == null ? default : new PurchaseOrderResponse(order);
    }

    public async Task<PurchaseOrderResponse> CreateOrderAsync(PurchaseOrderCreateRequest purchaseOrderCreateRequest)
    {
        var order = new PurchaseOrder();
        var items = await _catalogItemService
            .GetProductsByIdsAsync(purchaseOrderCreateRequest.Items.Select(x => x.ItemId)
                .ToArray());

        var verifyStock = from item in items
                          join itemOrder in purchaseOrderCreateRequest.Items on item.Id equals itemOrder.ItemId
                          where item.Quantity < itemOrder.Quantity
                          select item;

        if (verifyStock.Any())
        {
            throw new ArgumentException("Item(s) do pedido sem estoque.");
        }

        foreach (var item in items)
        {
            var itemOrder = purchaseOrderCreateRequest.Items.Single(x => x.ItemId == item.Id);
            order.AddPurchaseOrderItem(new PurchaseOrderItem(item.Description, item.Price, itemOrder.Quantity, itemOrder.ItemId));
        }

        await _orderDbContext.PurchaseOrders.AddAsync(order);
        await _orderDbContext.SaveChangesAsync();

        await _capPublisher.PublishAsync("ecomerce.payment.proccess", new PaymentMessage(purchaseOrderCreateRequest.CardName,
            purchaseOrderCreateRequest.CardNumber,
            purchaseOrderCreateRequest.ValidDate,
            purchaseOrderCreateRequest.Cvv,
            order.Id,
            order.TotalAmount));

        return new PurchaseOrderResponse(order);
    }

    public async Task<PurchaseOrderResponse> ReproccessOrderAsync(PurchaseOrderReproccessRequest purchaseOrderReproccessRequest)
    {
        var order = await _orderDbContext.PurchaseOrders
            .Include(x => x.PurchaseOrderItems)
            .SingleAsync(x => x.Id == purchaseOrderReproccessRequest.Id);

        if (order.OrderStatus == OrderStatus.Acepted)
        {
            throw new ArgumentException("Ordem com pagamento efetuado, não pode ser reprocessada");
        }

        var verifyStock = from item in order.PurchaseOrderItems
                          join itemOrder in purchaseOrderReproccessRequest.PurchaseOrderItemsCreateRequest on item.Id equals itemOrder.ItemId
                          where item.Quantity < itemOrder.Quantity
                          select item;

        if (verifyStock.Any())
        {
            throw new ArgumentException("Item(s) do pedido sem estoque.");
        }

        order.PurchaseOrderItems.Clear();
        order.TotalAmount = 0;
        order.OrderStatus = OrderStatus.Reprocessing;

        var items = await _catalogItemService
            .GetProductsByIdsAsync(purchaseOrderReproccessRequest.PurchaseOrderItemsCreateRequest.Select(x => x.ItemId)
                .ToArray());

        foreach (var item in items)
        {
            var itemOrder = purchaseOrderReproccessRequest.PurchaseOrderItemsCreateRequest.Single(x => x.ItemId == item.Id);
            order.AddPurchaseOrderItem(new PurchaseOrderItem(item.Description, item.Price, itemOrder.Quantity, itemOrder.ItemId));
        }

        _orderDbContext.PurchaseOrders.Update(order);
        await _orderDbContext.SaveChangesAsync();

        await _capPublisher.PublishAsync("ecomerce.payment.proccess", new PaymentMessage(purchaseOrderReproccessRequest.CardName,
            purchaseOrderReproccessRequest.CardNumber,
            purchaseOrderReproccessRequest.ValidDate,
            purchaseOrderReproccessRequest.Cvv,
            order.Id,
            order.TotalAmount));

        return await GetOrderByIdAsync(order.Id);
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
