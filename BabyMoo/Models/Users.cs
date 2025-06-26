using System.ComponentModel.DataAnnotations;

namespace BabyMoo.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

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
       

    }
}
