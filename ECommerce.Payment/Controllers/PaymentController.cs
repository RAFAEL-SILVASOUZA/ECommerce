using ECommerce.Payment.Domain.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Payment.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
       => _paymentService = paymentService;

    [HttpGet("{orderId}")]
    public async Task<Domain.Entities.Payment?> Get([FromRoute] Guid orderId)
       => await _paymentService.GetPaymentByOrderAsync(orderId);
}
