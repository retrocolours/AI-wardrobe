using System;
using System.Collections.Generic;

namespace AI_Wardrobe.Models;

public partial class OrderDetail
{
    public int Orderdetailsid { get; set; }

    public int? Fkitemid { get; set; }

    public DateOnly? Quantity { get; set; }

    public string? Price { get; set; }

    public int? Fkorderid { get; set; }

    public virtual Item? Fkitem { get; set; }

    public virtual Order? Fkorder { get; set; }
}
