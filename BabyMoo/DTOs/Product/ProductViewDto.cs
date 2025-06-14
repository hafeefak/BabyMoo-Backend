using BabyMoo.DTOs.Category;

namespace BabyMoo.DTOs.Product
{
    public class ProductViewDto
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }


        //public CategoryViewDto Category { get; set; }
    }
}
