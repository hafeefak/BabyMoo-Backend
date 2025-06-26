using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyMoo.Models
{
    public class Wishlist
    {
        [Key]
        public int WishlistId { get; set; }

        
        [Required]
        public int UserId { get; set; }
        public virtual User? User { get; set; }

    
            [Required]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
