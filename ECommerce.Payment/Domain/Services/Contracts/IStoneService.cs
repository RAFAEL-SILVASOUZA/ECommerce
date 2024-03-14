using ECommerce.Payment.Models;

namespace ECommerce.Payment.Domain.Services.Contracts;

public interface IStoneService
{
    Task<PaymentResponse> HandlerPaymentAsync(PaymentRequest paymentEntity);
}
