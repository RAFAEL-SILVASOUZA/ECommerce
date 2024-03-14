﻿using ECommerce.Payment.Domain.Services.Contracts;
using ECommerce.Payment.Models;
using Flurl;
using Flurl.Http;

namespace ECommerce.Payment.Domain.Services;

public class CieloService : ICieloService
{
    private readonly Url _baseUrl;

    public CieloService(IConfiguration configuration)
       => _baseUrl = configuration["Gateway:url"];

    public async Task<PaymentResponse> HandlerPaymentAsync(PaymentRequest paymentEntity)
    {
        var request = _baseUrl.AppendPathSegment("/cielo");
        var response = await request.PostJsonAsync(paymentEntity);
        var paymentResponseDto = await response.GetJsonAsync<PaymentResponse>();
        paymentResponseDto.GatewayName = "Cielo";
        return paymentResponseDto;
    }
}
