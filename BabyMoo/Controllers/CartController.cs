using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BabyMoo.Services.Cart;
using BabyMoo.DTOs.Cart;
using BabyMoo.Models;
using BabyMoo.Middleware;
using System.Security.Claims;

namespace BabyMoo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            int userId = GetUserId();
            var result = await _cartService.AddToCart(userId, productId, quantity);
            return Ok(new ApiResponse<CartViewDto>(200, "Item added to cart", result));
        }

        [HttpGet("mycart")]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                int userId = GetUserId();
                var result = await _cartService.GetCartItems(userId);
                return Ok(new ApiResponse<CartViewDto>(200, "Cart retrieved", result));
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new ApiResponse<string>(401, ex.Message));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "Internal server error"));
            }
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            int userId = GetUserId();
            var result = await _cartService.RemoveFromCart(userId, cartItemId);
            return Ok(new ApiResponse<CartViewDto>(200, "Item removed from cart", result));
        }

        [HttpPut("increase")]
        public async Task<IActionResult> IncreaseQuantity(int productId, int quantity)
        {
            int userId = GetUserId();
            var result = await _cartService.AddQuantity(userId, productId, quantity);
            return Ok(new ApiResponse<CartViewDto>(200, "Quantity increased", result));
        }

        [HttpPut("decrease")]
        public async Task<IActionResult> DecreaseQuantity(int productId, int quantity)
        {
            int userId = GetUserId();
            var result = await _cartService.ReduceQuantity(userId, productId, quantity);
            return Ok(new ApiResponse<CartViewDto>(200, "Quantity decreased", result));
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            int userId = GetUserId();
            var result = await _cartService.ClearCart(userId);
            return Ok(new ApiResponse<CartViewDto>(200, "Cart cleared", result));
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedException("User ID not found in token.");
            return int.Parse(userIdClaim.Value);
        }
    }
}
