using System.ComponentModel.DataAnnotations;

namespace AI_Wardrobe.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string? ProductName { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Price { get; set; }

        [Required]
        public string? Currency { get; set; }

        [Required]
        public string? Image { get; set; }
    }
}

