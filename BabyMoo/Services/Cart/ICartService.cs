using BabyMoo.DTOs.Cart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabyMoo.Services.Cart
{
    public interface ICartService
    {
        Task<CartViewDto?> AddToCart(int userId, int productId, int quantity);
        Task<CartViewDto?> GetCartItems(int userId);
        Task<CartViewDto?> RemoveFromCart(int userId, int productId);
        Task<CartViewDto?> AddQuantity(int userId, int productId, int quantity);
        Task<CartViewDto?> ReduceQuantity(int userId, int productId, int quantity);
        Task<CartViewDto?> ClearCart(int userId);
        Task<CartViewDto?> AllUsersCart(int userId, int productId, int quantity);
    }
}

