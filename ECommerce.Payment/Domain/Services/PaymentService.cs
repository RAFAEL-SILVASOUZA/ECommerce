using DotNetCore.CAP;
using ECommerce.Payment.Domain.Services.Contracts;
using ECommerce.Payment.Infra;
using ECommerce.Payment.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Payment.Domain.Services;

public class PaymentService : IPaymentService, ICapSubscribe
{
    private readonly PaymentDbContext _paymentDbContext;
    private readonly ICapPublisher _capPublisher;
    private readonly IPaymentGatewayService _paymentGatewayService;
    public PaymentService(PaymentDbContext paymentDbContext,
        ICapPublisher capPublisher,
        IPaymentGatewayService paymentGatewayService)
    {
        _paymentDbContext = paymentDbContext;
        _capPublisher = capPublisher;
        _paymentGatewayService = paymentGatewayService;
    }

    [CapSubscribe("ecomerce.payment.proccess")]
    public async Task ProccessPaymentAsync(PaymentRequest paymentEntity)
    {
        var paymentResponseDto = await _paymentGatewayService.HandlerPaymentAsync(paymentEntity);
        paymentResponseDto.OrderId = paymentEntity.OrderId;

        await _paymentDbContext.Payments.AddAsync(new Entities.Payment
        {
            Amount = paymentEntity.Amount,
            GatewayName = paymentResponseDto.GatewayName,
            OrderId = paymentResponseDto.OrderId,
            Description = paymentResponseDto.Description,
            PaymentStatus = paymentResponseDto.PaymentStatus,
            ProccessDate = paymentResponseDto.ProccessDate,
            TranzactionId = paymentResponseDto.TranzactionId
        });

        await _paymentDbContext.SaveChangesAsync();
        await _capPublisher.PublishAsync("ecomerce.order.status.payment", paymentResponseDto);
    }

    public async Task<Entities.Payment?> GetPaymentByOrderAsync(Guid orderId)
       => await _paymentDbContext
                .Payments
                .Where(x => x.OrderId == orderId)
                .FirstOrDefaultAsync();
}
