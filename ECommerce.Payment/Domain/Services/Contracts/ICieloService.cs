using ECommerce.Payment.Models;

namespace ECommerce.Payment.Domain.Services.Contracts;

public interface ICieloService
{
    Task<PaymentResponse> HandlerPaymentAsync(PaymentRequest paymentEntity);
}
