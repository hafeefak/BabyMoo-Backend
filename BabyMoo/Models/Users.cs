using System.ComponentModel.DataAnnotations;
using CloudinaryDotNet.Actions;

namespace BabyMoo.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User";

        public bool Blocked { get; set; } = false;

        public Guid Salt { get; set; } = Guid.NewGuid();
        public virtual CartModel? Carts{ get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new HashSet<Wishlist>();


        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
        public virtual ICollection<Order> Order { get; set; } = new HashSet<Order>();


    }
}
