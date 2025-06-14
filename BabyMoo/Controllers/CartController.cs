using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BabyMoo.Services.Cart;
using BabyMoo.DTOs.Cart;
using System.Security.Claims;


namespace BabyMoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

       
        [Authorize]
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
           
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            int userId = int.Parse(userIdClaim.Value);

            await _cartService.AddToCart(userId, productId, quantity);
            return Ok();
        }


        [Authorize]
        [HttpGet("mycart")]
        public async Task<IActionResult> GetCart()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _cartService.GetCartItems(userId);
            return result == null ? NotFound("Cart not found.") : Ok(result);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found.");

            int userId = int.Parse(userIdClaim.Value);

            var result = await _cartService.RemoveFromCart(userId, cartItemId);
            return result == null ? NotFound("Item not found.") : Ok(result);
        }



        [Authorize]
        [HttpPut("increase")]
        public async Task<IActionResult> IncreaseQuantity(int productId, int quantity)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            int userId = int.Parse(userIdClaim.Value);

            var result = await _cartService.AddQuantity(userId, productId, quantity);
            return result == null
                ? BadRequest("Failed to increase quantity.")
                : Ok(result);
        }


        [Authorize]
        [HttpPut("decrease")]
        public async Task<IActionResult> DecreaseQuantity(int productId, int quantity)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            int userId = int.Parse(userIdClaim.Value);

            var result = await _cartService.ReduceQuantity(userId, productId, quantity);
            return result == null
                ? BadRequest("Failed to decrease quantity.")
                : Ok(result);
        }

        [Authorize]
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            int userId = int.Parse(userIdClaim.Value);

            var result = await _cartService.ClearCart(userId);
            return result == null
                ? NotFound("Cart already empty or does not exist.")
                : Ok(result);
        }

    }
}


