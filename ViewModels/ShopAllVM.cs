using System.ComponentModel.DataAnnotations;

namespace AI_Wardrobe.ViewModels
{
    public class ShopAllVM
    {
        public int ID { get; set; }

        [Display(Name = "Product Name")]
        public string? ProductName { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Price")]
        public string? Price { get; set; }

        [Display(Name = "Currency")]
        public string? Currency { get; set; }

        [Display(Name = "Image")]
        public string? Image { get; set; }
    }
}
