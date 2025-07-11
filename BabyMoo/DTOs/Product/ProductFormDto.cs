using Microsoft.AspNetCore.Http;

namespace BabyMoo.DTOs.Product
{
    public class ProductFormDto
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; } = string.Empty;


        public IFormFile? Image { get; set; }
        // ✅ Combine image here
    }
}
