using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyMoo.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PinCode { get; set; }

        [Required]
        public string Country { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
