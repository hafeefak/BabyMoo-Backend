using BabyMoo.DTOs.Product;
using BabyMoo.Models;
using Microsoft.AspNetCore.Http;

namespace BabyMoo.Services.Products
{
    public interface IProductService
    {
        Task<List<ProductViewDto>> GetAllProducts();
        Task<ProductViewDto> GetProductsById(int id);
        Task<List<ProductViewDto>> GetProductByCategory(string category);
        Task<ProductViewDto> CreateProduct(ProductDto dto, IFormFile image);
        Task<bool> UpdateProduct(int id, ProductDto dto, IFormFile image);
        Task<bool> DeleteProduct(int id);
        Task<List<ProductViewDto>> SearchProduct(string text);
    }
}
