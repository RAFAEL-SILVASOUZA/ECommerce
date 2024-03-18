namespace ECommerce.Order.Worker.Models.Response;

public class PurchaseOrderPaymentResponse
{
    public Guid OrderId { get; set; }
    public Guid TranzactionId { get; set; }
    public DateTime ProccessDate { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string GatewayName { get; set; }

}

public enum PaymentStatus
{
    Accepted,
    Rejected
}
