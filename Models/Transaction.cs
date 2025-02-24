using System;

namespace AI_Wardrobe.Models
{
    public class Transaction
    {
        public int Transactionid { get; set; } 

        // public string? PayPalTransactionId { get; set; } 

        public decimal? Totalamount { get; set; } 

        public DateTime? Transactiondate { get; set; }  

        public string? Transactionstatus { get; set; } 

        // public string? PayerName { get; set; } 

        // public string? PayerEmail { get; set; } 
        // public string? Currency { get; set; } 

        // public string? PaymentMethod { get; set; } 

        public int? Fkorderid { get; set; } 

        public virtual Order? Fkorder { get; set; } 
    }
}
