using ECommerce.Payment.Domain.Services.Contracts;
using ECommerce.Payment.Infra;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Payment.Domain.Services;

public class PaymentService : IPaymentService
{
    private readonly PaymentDbContext _paymentDbContext;
    public PaymentService(PaymentDbContext paymentDbContext)
    {
        _paymentDbContext = paymentDbContext;
    }

    public async Task<Entities.Payment?> GetPaymentByOrderAsync(Guid orderId)
       => await _paymentDbContext
                .Payments
                .Where(x => x.OrderId == orderId)
                .FirstOrDefaultAsync();
}
