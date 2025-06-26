using BabyMoo.DTOs.Product;



namespace BabyMoo.Services.Wishlists
{
    public interface IWishlistService
    {
        Task<bool> AddToWishlist(int userId, int productId);
        Task<List<ProductViewDto>> GetWishlist(int userId);
        Task<bool> RemoveFromWishlist(int userId, int productId);
    }
}