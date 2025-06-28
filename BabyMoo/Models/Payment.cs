using System.ComponentModel.DataAnnotations;

namespace BabyMoo.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relation
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
