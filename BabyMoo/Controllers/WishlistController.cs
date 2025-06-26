using BabyMoo.DTOs.Product;
using BabyMoo.Models;
using BabyMoo.Services.Wishlists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BabyMoo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        // ✅ Get userId from token claims
        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        // ✅ Add product to wishlist
        [HttpPost("{productId}")]
        public async Task<IActionResult> Add(int productId)
        {
            var userId = GetUserId();
            var result = await _wishlistService.AddToWishlist(userId, productId);

            if (!result)
                return BadRequest(new ApiResponse<string>(400, "Product already exists in wishlist"));

            return Ok(new ApiResponse<string>(200, "Product added to wishlist"));
        }

        // ✅ Get all wishlist products for the user
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = GetUserId();
            var wishlist = await _wishlistService.GetWishlist(userId);

            return Ok(new ApiResponse<List<ProductViewDto>>(200, "Wishlist fetched successfully", wishlist));
        }

        // ✅ Remove product from wishlist
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            var userId = GetUserId();
            var result = await _wishlistService.RemoveFromWishlist(userId, productId);

            if (!result)
                return NotFound(new ApiResponse<string>(404, "Product not found in wishlist"));

            return Ok(new ApiResponse<string>(200, "Product removed from wishlist"));
        }
    }
}
