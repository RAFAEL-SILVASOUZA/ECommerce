using ECommerce.Payment.Domain.Entities.Enums;

namespace ECommerce.Payment.Models
{
    public class PaymentResponse
    {
        public Guid OrderId { get; set; }
        public Guid TranzactionId { get; set; }
        public DateTime ProccessDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string GatewayName { get; set; }

    }
}
