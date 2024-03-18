using ECommerce.Payment.Models;
using ECommerce.Payment.Worker.Models;

namespace ECommerce.Payment.Domain.Services.Contracts;

public interface IStoneService
{
    Task<PaymentResponse> HandlerPaymentAsync(PaymentRequest paymentEntity);
}
