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

    public virtual Order? Fkorder { get; set; }
}
