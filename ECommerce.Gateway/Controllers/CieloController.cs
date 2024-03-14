using ECommerce.Gateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CieloController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(PaymentRequest paymentRequest)
       => Ok(new PaymentResponse(paymentRequest, GatewayEnum.Cielo));
}
