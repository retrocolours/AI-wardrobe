using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Display(Name = "Deliver To")]
        public string? OrderedBy { get; set; }

        [Display(Name = "Delivery Address")]
        public string? DeliverAddress { get; set; }
       
        public TransactionVM? TransactionVM { get; set; }

        public IEnumerable<OrderDetailVM>? OrderDetailVMs;

        //Drop down options
        public List<SelectListItem>? StatusOptions;

        //Image usl(s) for disaplying in order history, hould be the usrl of the items in the order
        [ValidateNever]
        public List<string> Images { get; set; }
    }
}
