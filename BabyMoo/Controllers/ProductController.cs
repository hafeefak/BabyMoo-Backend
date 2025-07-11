using BabyMoo.DTOs.Product;
using BabyMoo.Middleware;
using BabyMoo.Models;
using BabyMoo.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyMoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(new ApiResponse<List<ProductViewDto>>(200, "Product list fetched", products));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductsById(id);
            return Ok(new ApiResponse<ProductViewDto>(200, "Product fetched", product));
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var products = await _productService.GetProductByCategory(category);
            return Ok(new ApiResponse<List<ProductViewDto>>(200, $"Products in category '{category}'", products));
        }

        [HttpGet("search/{text}")]
        public async Task<IActionResult> Search(string text)
        {
            var results = await _productService.SearchProduct(text);
            return Ok(new ApiResponse<List<ProductViewDto>>(200, $"Search results for '{text}'", results));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductFormDto form)
        {
            if (form.Image == null || form.Image.Length == 0)
                throw new BadRequestException("Image is required");

            var dto = new ProductDto
            {
                ProductName = form.ProductName,
                Description = form.Description,
                Price = form.Price,
                Quantity = form.Quantity,
                CategoryName = form.CategoryName
            };

            await _productService.CreateProduct(dto, form.Image);
            return Ok(new ApiResponse<string>(200, "Product added successfully"));
        }

       
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductFormDto form)
        {
            // Image should be optional in update; keep existing if null
            var dto = new ProductDto
            {
                ProductName = form.ProductName,
                Description = form.Description,
                Price = form.Price,
                Quantity = form.Quantity,
                CategoryName = form.CategoryName
            };

            await _productService.UpdateProduct(id, dto, form.Image);
            return Ok(new ApiResponse<string>(200, "Product updated successfully"));
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);
            return Ok(new ApiResponse<string>(200, "Product deleted successfully"));
        }
    }
}
