using ECommerce.Payment.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentDbContext _paymentDbContext;

        public PaymentController(PaymentDbContext paymentDbContext)
        {
            _paymentDbContext = paymentDbContext;
        }

        [HttpGet("orderId")]
        public async Task<IActionResult> Get([FromQuery] Guid orderId)
        {
            var payments = await _paymentDbContext
                .Payments
                .Where(x => x.OrderId == orderId)
                .FirstOrDefaultAsync();

            return Ok(payments);
        }
    }
}
