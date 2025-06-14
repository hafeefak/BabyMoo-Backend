using BabyMoo.Services.Product;
using Microsoft.AspNetCore.Mvc;
using BabyMoo.DTOs.Product;

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

        // ✅ GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        // ✅ GET: api/product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductsById(id);
            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        // ✅ POST: api/product
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductViewDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _productService.AddProduct(productDto);
                if (result)
                    return Ok("Product added successfully.");
                else
                    return BadRequest("Product could not be added.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
