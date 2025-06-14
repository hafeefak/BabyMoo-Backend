using BabyMoo.DTOs.Product;

namespace BabyMoo.Services.Product
{
    public interface IProductService
    {
        Task<List<ProductViewDto>> GetAllProducts();
        Task<ProductViewDto> GetProductsById(int id);
        Task<bool> AddProduct(ProductViewDto productDto);

    }
}
