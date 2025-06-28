using System.ComponentModel.DataAnnotations;

namespace BabyMoo.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending"; // use this for "Completed"

        public string PaymentStatus { get; set; } = "Pending";
        public string? PaymentMethod { get; set; }    // Paypal / COD etc
        public string? PaymentReferenceId { get; set; }

        public string? PaymentToken { get; set; } // ✅ new field for fake token

        // Relations
        public int UserId { get; set; }
        public User User { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
