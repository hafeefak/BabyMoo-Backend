using System.ComponentModel.DataAnnotations;

namespace BabyMoo.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        // Quantity & Price
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Foreign keys
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
