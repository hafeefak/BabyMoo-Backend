using System.ComponentModel.DataAnnotations;

namespace BabyMoo.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending"; 

        public string PaymentStatus { get; set; } = "Pending";
        public string? PaymentMethod { get; set; }  
        public string? PaymentReferenceId { get; set; }

        public string? PaymentToken { get; set; } 

    
        public int UserId { get; set; }
        public User User { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
