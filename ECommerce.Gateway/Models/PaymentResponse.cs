namespace ECommerce.Gateway.Models;

public class PaymentResponse
{
    public PaymentResponse(PaymentRequest payment, GatewayEnum gatewayEnum)
    {
        TranzactionId = Guid.NewGuid();
        ProccessDate = DateTime.Now;
        Amount = payment.Amount;
        PaymentStatus = GetStatus(payment.Amount, gatewayEnum);
        Description = GetDescrition();
    }

    private PaymentStatus GetStatus(decimal amount, GatewayEnum gatewayEnum)
    {
        switch (gatewayEnum)
        {
            case GatewayEnum.Cielo:
                return amount <= 4000 ? PaymentStatus.Accepted : PaymentStatus.Rejected;
            case GatewayEnum.Stone:
                return (amount > 4000 && amount <= 6000) ? PaymentStatus.Accepted : PaymentStatus.Rejected;
            default:
                return PaymentStatus.Rejected;
        }
    }

    private string GetDescrition()
    {
        switch (PaymentStatus)
        {
            case PaymentStatus.Rejected:
                return "Insufficient balance";
            case PaymentStatus.Accepted:
                return "Balance consumed";
            default:
                return "";
        }
    }

    public Guid TranzactionId { get; set; }
    public DateTime ProccessDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}