using System.ComponentModel.DataAnnotations;

namespace AI_Wardrobe.ViewModels
{
    public class OrderDetailVM
    {
        [Display(Name = "Order Detail ID")]
        public int Id { get; set; }

        [Display(Name = "Quantity")]
        public int? Quantity { get; set; }

        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        public ProductVM? productVM { get; set; }

    }
}
