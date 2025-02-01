using System.ComponentModel.DataAnnotations;

namespace AI_Wardrobe.ViewModels
{
    public class OrderVM
    {
        [Display(Name = "Order ID")]
        public int Id { get; set; }

        [Display(Name = "Order Date")]
        public DateOnly? Date { get; set; }

        [Display(Name = "Order Status")]
        public string? Status { get; set; }

        [Display(Name = "Ordered By")]
        public string? OrderedBy { get; set; }

        [Display(Name = "Delievery Address")]
        public string? DeliverAddress { get; set; }
       
        public TransactionVM? TransactionVM { get; set; }

        public IEnumerable<OrderDetailVM>? OrderDetailVMs;
    }
}
