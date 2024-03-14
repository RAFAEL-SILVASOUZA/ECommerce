using ECommerce.Payment.Domain.Services.Contracts;
using ECommerce.Payment.Models;
using Flurl;
using Flurl.Http;

namespace ECommerce.Payment.Domain.Services;

public class StoneService : IStoneService
{
    private readonly Url _baseUrl;

    public StoneService(IConfiguration configuration)
       => _baseUrl = configuration["Gateway:url"];
 
    public async Task<PaymentResponse> HandlerPaymentAsync(PaymentRequest paymentEntity)
    {
        var request = _baseUrl.AppendPathSegment("/stone");
        var response = await request.PostJsonAsync(paymentEntity);
        var paymentResponseDto = await response.GetJsonAsync<PaymentResponse>();
        paymentResponseDto.GatewayName = "Stone";
        return paymentResponseDto;
    }
}
