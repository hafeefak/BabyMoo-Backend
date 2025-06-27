using AutoMapper;
using BabyMoo.CloudinaryService;
using BabyMoo.Data;
using BabyMoo.DTOs.Product;
using BabyMoo.Middleware;
using BabyMoo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace BabyMoo.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, ICloudinaryService cloudinaryService, IMapper mapper)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }

        public async Task<List<ProductViewDto>> GetAllProducts()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return _mapper.Map<List<ProductViewDto>>(products);
        }

        public async Task<ProductViewDto> GetProductsById(int id)
        {
            var product = await _context.Products.Include(c => c.Category)
                                                 .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
                throw new NotFoundException($"Product with ID {id} not found.");

            return _mapper.Map<ProductViewDto>(product);
        }

        public async Task<List<ProductViewDto>> GetProductByCategory(string category)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Category.CategoryName.ToLower() == category.ToLower())
                .ToListAsync();

            return _mapper.Map<List<ProductViewDto>>(products);
        }

        public async Task<ProductViewDto> CreateProduct(ProductDto dto, IFormFile image)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == dto.CategoryName);
            if (category == null)
                throw new BadRequestException("Invalid category name.");

            string imageUrl = await _cloudinaryService.UploadImageAsync(image);

            var product = _mapper.Map<Product>(dto);
            product.ImageUrl = imageUrl;
            product.CategoryId = category.CategoryId;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var created = await _context.Products.Include(c => c.Category)
                                                 .FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

            return _mapper.Map<ProductViewDto>(created);
        }

        public async Task<bool> UpdateProduct(int id, ProductDto dto, IFormFile image)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new NotFoundException($"Product with ID {id} not found.");

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == dto.CategoryName);
            if (category == null)
                throw new BadRequestException("Invalid category name.");

            if (image != null)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(image);
                product.ImageUrl = imageUrl;
            }

            product.ProductName = dto.ProductName;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Quantity = dto.Quantity;
            product.CategoryId = category.CategoryId;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new NotFoundException($"Product with ID {id} not found.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductViewDto>> SearchProduct(string text)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.ProductName.Contains(text) || p.Description.Contains(text))
                .ToListAsync();

            return _mapper.Map<List<ProductViewDto>>(products);
        }
    }
}
