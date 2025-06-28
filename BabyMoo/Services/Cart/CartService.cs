using AutoMapper;
using BabyMoo.Data;
using BabyMoo.DTOs.Cart;
using BabyMoo.Models;
using BabyMoo.Middleware;
using Microsoft.EntityFrameworkCore;

namespace BabyMoo.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CartService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartViewDto> AddToCart(int userId, int productId, int quantity)
        {
            var cart = await GetOrCreateCart(userId);
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
                throw new NotFoundException("Product not found");

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = existingItem.Quantity * product.Price;
            }
            else
            {
                var item = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    TotalPrice = quantity * product.Price
                };
                _context.CartItems.Add(item);
            }

            await _context.SaveChangesAsync();
            return await GetCartItems(userId);
        }

        public async Task<CartViewDto> GetCartItems(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                throw new NotFoundException("Cart is empty or not found");

            return new CartViewDto
            {
                Items = cart.CartItems.Select(i => new CartItemDto
                {
                    CartItemId = i.Id,
                    ProductName = i.Product?.ProductName ?? "",
                    Quantity = i.Quantity,
                    ProductId = i.ProductId,
                    Price = i.Product?.Price ?? 0,
                    TotalPrice = i.TotalPrice,
                    ImageUrl = i.Product?.ImageUrl ?? ""
                }).ToList(),
                TotalAmount = cart.CartItems.Sum(i => i.TotalPrice)
            };
        }

        public async Task<CartViewDto> RemoveFromCart(int userId, int cartItemId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                throw new NotFoundException("Cart not found");

            var item = cart.CartItems.FirstOrDefault(i => i.Id == cartItemId);
            if (item == null)
                throw new NotFoundException("Item not found in cart");

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return await GetCartItems(userId);
        }

        public async Task<CartViewDto> AddQuantity(int userId, int productId, int quantity)
        {
            return await AddToCart(userId, productId, quantity);
        }

        public async Task<CartViewDto> ReduceQuantity(int userId, int productId, int quantity)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                throw new NotFoundException("Cart not found");

            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new NotFoundException("Product not found in cart");

            item.Quantity -= quantity;

            if (item.Quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                var product = await _context.Products.FindAsync(productId);
                item.TotalPrice = item.Quantity * (product?.Price ?? 0);
            }

            await _context.SaveChangesAsync();
            return await GetCartItems(userId);
        }

        public async Task<CartViewDto> ClearCart(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                throw new NotFoundException("Cart already empty or does not exist");

            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return new CartViewDto { Items = new List<CartItemDto>(), TotalAmount = 0 };
        }

        public async Task<CartViewDto> AllUsersCart(int userId, int productId, int quantity)
        {
            return await AddToCart(userId, productId, quantity);
        }

        private async Task<CartModel> GetOrCreateCart(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                throw new NotFoundException($"User with ID {userId} does not exist");

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new CartModel { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }
    }
}
