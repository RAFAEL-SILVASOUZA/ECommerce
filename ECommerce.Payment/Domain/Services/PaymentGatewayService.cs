
using ECommerce.Payment.Domain.Services.Contracts;
using ECommerce.Payment.Models;

namespace ECommerce.Payment.Domain.Services;
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

        if (response.PaymentStatus == Entities.Enums.PaymentStatus.Rejected)
            response = await _stoneService.HandlerPaymentAsync(paymentEntity);

        if (response.PaymentStatus == Entities.Enums.PaymentStatus.Rejected)
            response.GatewayName = "N/A";

        return response;
    }
}
