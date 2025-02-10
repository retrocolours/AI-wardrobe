using System;
using System.ComponentModel.DataAnnotations;

namespace AI_Wardrobe.ViewModels
{
    public class TransactionVM
    {
        [Display(Name = "Transaction ID")]
        public string? TransactionId { get; set; } 

        [Display(Name = "Created")]
        public DateTime? CreateTime { get; set; } 

        [Display(Name = "Name")]
        public string? PayerName { get; set; }

        [Display(Name = "Email")]
        public string? PayerEmail { get; set; } 

        [Display(Name = "Amount")]
        public decimal? Amount { get; set; } 

        [Display(Name = "Currency")]
        public string? Currency { get; set; } 

        [Display(Name = "Payment Method")]
        public string? PaymentMethod { get; set; } 

        [Display(Name = "Transaction Status")]
        public string? Transactionstatus { get; set; } 
}
}