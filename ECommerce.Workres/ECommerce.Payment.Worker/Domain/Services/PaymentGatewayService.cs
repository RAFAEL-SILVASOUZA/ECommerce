using ECommerce.Payment.Domain.Services.Contracts;
using ECommerce.Payment.Models;
using ECommerce.Payment.Worker.Domain.Entities.Enums;
using ECommerce.Payment.Worker.Domain.Services.Contracts;
using ECommerce.Payment.Worker.Models;

namespace ECommerce.Payment.Worker.Domain.Services;
public class PaymentGatewayService : IPaymentGatewayService
{
    private readonly ICieloService _cieloService;
    private readonly IStoneService _stoneService;

    public PaymentGatewayService(ICieloService cieloService, IStoneService stoneService)
    {
        _cieloService = cieloService;
        _stoneService = stoneService;
    }
    public async Task<PaymentResponse> HandlerPaymentAsync(PaymentRequest paymentEntity)
    {
        var response = await _cieloService.HandlerPaymentAsync(paymentEntity);

        if (response.PaymentStatus == PaymentStatus.Rejected)
            response = await _stoneService.HandlerPaymentAsync(paymentEntity);

        if (response.PaymentStatus == PaymentStatus.Rejected)
            response.GatewayName = "N/A";

        return response;
    }
}
