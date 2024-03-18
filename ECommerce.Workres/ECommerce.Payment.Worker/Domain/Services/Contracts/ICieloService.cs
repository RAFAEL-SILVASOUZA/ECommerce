using ECommerce.Payment.Models;
using ECommerce.Payment.Worker.Models;

namespace ECommerce.Payment.Worker.Domain.Services.Contracts;

public interface ICieloService
{
    Task<PaymentResponse> HandlerPaymentAsync(PaymentRequest paymentEntity);
}
