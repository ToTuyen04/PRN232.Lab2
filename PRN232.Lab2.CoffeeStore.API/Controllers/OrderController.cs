using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.Lab2.CoffeeStore.Services.RequestModel;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using PRN232.Lab2.CoffeeStore.Services.Service.IService;

namespace PRN232.Lab2.CoffeeStore.API.Controllers
{
    [ApiController]
    [Route("/api/orders")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id,[FromBody] OrderUpdateStatusRequest request)
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, request);
            return Ok(SuccessResponse<OrderResponse>.Create(order, "Update order status success."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(SuccessResponse<OrderResponse>.Create(order, "Get order success."));
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string? username, 
            [FromQuery] string? paymentMethod, 
            [FromQuery] string? select, 
            [FromQuery] string? orderBy, 
            [FromQuery] int currentPage = 1, 
            [FromQuery] int pageSize = 5)
        {
            var orders = await _orderService.GetAllOrdersAsync(username, paymentMethod, select, orderBy, currentPage, pageSize);
            return Ok(SuccessResponse<Paginated<OrderResponse>>.Create(orders, "Get orders success."));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequest request)
        {
            var order = await _orderService.PlaceOrderAsync(request);
            return CreatedAtAction(
                nameof(Post),
                new { id = order.OrderId },
                SuccessResponse<OrderResponse>.Create(order, "Order placed success."));
        }
    }
}
