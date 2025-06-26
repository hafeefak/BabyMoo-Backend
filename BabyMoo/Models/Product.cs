using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BabyMoo.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
