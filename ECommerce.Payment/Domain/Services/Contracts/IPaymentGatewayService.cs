using ECommerce.Payment.Models;

namespace ECommerce.Payment.Domain.Services.Contracts;

public interface IPaymentGatewayService
{
    Task<PaymentResponse> HandlerPaymentAsync(PaymentRequest paymentEntity);
}
