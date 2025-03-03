using System;
using System.Collections.Generic;

namespace AI_Wardrobe.Models;

public partial class Transaction
{
    public int Transactionid { get; set; }

    public decimal? Totalamount { get; set; }

    public DateOnly? Transactiondate { get; set; }

    public string? Transactionstatus { get; set; }

    public int? Fkorderid { get; set; }

    public string? Paypaltransactionid { get; set; }

    public string? Payername { get; set; }

    public string? Payeremail { get; set; }

    public string? Currency { get; set; }

    public string? Paymentmethod { get; set; }

    public virtual Order? Fkorder { get; set; }
}
