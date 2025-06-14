using System.ComponentModel.DataAnnotations;

namespace BabyMoo.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50)]
        public string CategoryName { get; set; }  
        public virtual List<Product>? Products { get; set; }
    }
}
