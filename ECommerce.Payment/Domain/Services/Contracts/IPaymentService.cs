namespace ECommerce.Payment.Domain.Services.Contracts;

public interface IPaymentService
{
    Task<Entities.Payment?> GetPaymentByOrderAsync(Guid orderId);
}
