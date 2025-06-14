using AutoMapper;
using BabyMoo.Data;
using BabyMoo.DTOs.Product;
using BabyMoo.Models;
using Microsoft.EntityFrameworkCore;

namespace BabyMoo.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ✅ Get all products
        public async Task<List<ProductViewDto>> GetAllProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            return _mapper.Map<List<ProductViewDto>>(products);
        }

        // ✅ Get product by Id
        public async Task<ProductViewDto> GetProductsById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            return _mapper.Map<ProductViewDto>(product)!;
        }

        // ✅ Add product
        public async Task<bool> AddProduct(ProductViewDto productDto)
        {
            if (productDto == null)
                throw new ArgumentNullException(nameof(productDto));

            // Find the category by name
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == productDto.CategoryName.ToLower());

            if (category == null)
                throw new Exception("Category does not exist.");

            // Map the ProductViewDto to Product entity
            var product = _mapper.Map<Models.Product>(productDto);
            product.CategoryId = category.CategoryId;

            // Save to database
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
