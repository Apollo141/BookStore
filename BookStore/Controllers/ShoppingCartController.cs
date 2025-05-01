using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;

        public ShoppingCartController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }
        [Authorize(Policy = "UserOrAdmin")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromQuery] string userId, [FromQuery] int bookId, [FromQuery] int quantity)
        {
            await _cartService.AddItemAsync(userId, bookId, quantity);
            return Ok(new { Message = "Item added to cart" });
        }
        [Authorize(Policy = "UserOrAdmin")]
        [HttpDelete("remove")]
        public async Task<IActionResult> Remove([FromQuery] string userId, [FromQuery] int bookId)
        {
            await _cartService.RemoveItemAsync(userId, bookId);
            return Ok(new { Message = "Item removed from cart" });
        }
        [Authorize(Policy = "UserOrAdmin")]
        [HttpGet]
        public async Task<ActionResult<CartDto>> Get([FromQuery] string userId)
        {
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }
        [Authorize(Policy = "UserOrAdmin")]
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromQuery] string userId)
        {
            await _cartService.CheckoutAsync(userId);
            return Ok(new { Message = "Checkout complete" });
        }
    }
}
