using System;

namespace AI_Wardrobe.Models
{
    public class Transaction
    {
        public int Transactionid { get; set; } // Unique ID for transaction

        public string? PayPalTransactionId { get; set; } // PayPal Transaction ID

        public decimal? Totalamount { get; set; } // Transaction Amount

        public DateTime? Transactiondate { get; set; }  // Date of transaction

        public string? Transactionstatus { get; set; } // Completed, Failed, etc.

        public string? PayerName { get; set; } // Payer's Name from PayPal

        public string? PayerEmail { get; set; } // Payer's Email from PayPal

        public string? Currency { get; set; } // Currency of the transaction (USD, CAD, etc.)

        public string? PaymentMethod { get; set; } // PayPal, Credit Card, etc.

        public int? Fkorderid { get; set; } // Foreign key to Order table

        public virtual Order? Fkorder { get; set; } // Relationship with Order
    }
}
