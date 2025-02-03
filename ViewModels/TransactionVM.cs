using System.ComponentModel.DataAnnotations;

namespace AI_Wardrobe.ViewModels
{
    public class TransactionVM
    {
        [Display(Name = "Transaction ID")]
        public string Id { get; set; } = null!;

        [Display(Name = "Created")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "Name")]
        public string? PayerName { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Amount")]
        public decimal? Amount { get; set; }

        [Display(Name = "Payment Mode")]
        public string? PaymentMode { get; set; }
    }
}
