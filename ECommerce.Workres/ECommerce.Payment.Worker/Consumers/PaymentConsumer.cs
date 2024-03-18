using DotNetCore.CAP;
using ECommerce.Payment.Domain.Services.Contracts;
using ECommerce.Payment.Worker.Infra;
using ECommerce.Payment.Worker.Models;

namespace ECommerce.Payment.Worker.Consumers
{
    public interface IPaymentConsumer { }
    public class PaymentConsumer : IPaymentConsumer, ICapSubscribe
    {

        private readonly PaymentDbContext _paymentDbContext;
        private readonly ICapPublisher _capPublisher;
        private readonly IPaymentGatewayService _paymentGatewayService;
        public PaymentConsumer(PaymentDbContext paymentDbContext,
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

            await _paymentDbContext.Payments.AddAsync(new Domain.Entities.Payment
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
    }
}
