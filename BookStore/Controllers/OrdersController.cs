using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize(Policy = "UserOrAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> Get(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }
        [Authorize(Policy = "UserOrAdmin")]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<OrderDto>>> GetForUser(string userId)
        {
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }
    }
}
