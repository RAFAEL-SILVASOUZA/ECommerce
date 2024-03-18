using ECommerce.Order.Worker.Domain.Entities.Enums;

namespace ECommerce.Order.Domain.Entities;

public class PurchaseOrder
{
    public PurchaseOrder()
    {
        PurchaseOrderItems = new HashSet<PurchaseOrderItem>();
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        OrderStatus = OrderStatus.Created;
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public string? GatewayName { get; set; }
    public Guid TranzactionId { get; set; }
    public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }

    public void AddPurchaseOrderItem(PurchaseOrderItem purchaseOrderItem)
    {
        PurchaseOrderItems.Add(purchaseOrderItem);
        TotalAmount += purchaseOrderItem.Price * purchaseOrderItem.Quantity;
    }
}
