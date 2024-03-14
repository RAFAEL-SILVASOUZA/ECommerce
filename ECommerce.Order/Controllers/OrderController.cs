using ECommerce.Order.Domain.Services.Contracts;
using ECommerce.Order.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public OrderController(IPurchaseOrderService purchaseOrderService)
           => _purchaseOrderService = purchaseOrderService;

        [HttpPost]
        public async Task<IActionResult> Post(PurchaseOrderCreateRequest purchaseOrderCreateRequest)
           => Ok(await _purchaseOrderService.CreateOrderAsync(purchaseOrderCreateRequest));

        [HttpPost("reproccess")]
        public async Task<IActionResult> Post(PurchaseOrderReproccessRequest purchaseOrderReproccessRequest)
           => Ok(await _purchaseOrderService.ReproccessOrderAsync(purchaseOrderReproccessRequest));

        [HttpGet]
        public async Task<IActionResult> Get()
           => Ok(await _purchaseOrderService.GetAllOrdersAsync());


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var order = await _purchaseOrderService.GetOrderByIdAsync(id);
            return order != null ? Ok(order) : NotFound();
        }
    }
}
