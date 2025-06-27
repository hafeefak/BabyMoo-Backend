using AutoMapper;
using BabyMoo.Data;
using BabyMoo.DTOs.Product;
using BabyMoo.Middleware;

using BabyMoo.Models;
using Microsoft.EntityFrameworkCore;

namespace BabyMoo.Services.Wishlists
{
    public class WishlistService : IWishlistService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public WishlistService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddToWishlist(int userId, int productId)
        {
            var exists = await _context.Wishlists
                .AnyAsync(w => w.UserId == userId && w.ProductId == productId);

            if (exists)
                throw new BadRequestException("Product already exists in wishlist");

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new NotFoundException("Product does not exist");

            var wishlistItem = new Wishlist
            {
                UserId = userId,
                ProductId = productId
            };

            _context.Wishlists.Add(wishlistItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductViewDto>> GetWishlist(int userId)
        {
            var wishlistProducts = await _context.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Product)
                    .ThenInclude(p => p.Category)
                .Select(w => w.Product)
                .ToListAsync();

            return _mapper.Map<List<ProductViewDto>>(wishlistProducts);
        }

        public async Task<bool> RemoveFromWishlist(int userId, int productId)
        {
            var item = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (item == null)
                throw new NotFoundException("Product not found in wishlist");

            _context.Wishlists.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
