namespace BabyMoo.DTOs.Product
{
    public class ProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }

    }
}
