using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BabyMoo.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }
        [Required]
        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public virtual CartModel? Cart { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
