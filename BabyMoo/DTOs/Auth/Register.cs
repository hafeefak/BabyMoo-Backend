using System.ComponentModel.DataAnnotations;

namespace BabyMoo.DTOs.Auth
{
    public class Register
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
