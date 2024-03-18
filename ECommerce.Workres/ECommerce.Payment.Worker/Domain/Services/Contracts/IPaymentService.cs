using ECommerce.Payment.Worker.Models;

namespace ECommerce.Payment.Domain.Services.Contracts;

public interface IPaymentService
{
    Task ProccessPaymentAsync(PaymentRequest paymentEntity);
}
