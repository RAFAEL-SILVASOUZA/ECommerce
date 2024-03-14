using ECommerce.Payment.Models;

namespace ECommerce.Payment.Domain.Services.Contracts;

public interface IPaymentService
{
    Task ProccessPaymentAsync(PaymentRequest paymentEntity);
    Task<Entities.Payment?> GetPaymentByOrderAsync(Guid orderId);
}
