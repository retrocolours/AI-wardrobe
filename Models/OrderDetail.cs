using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AI_Wardrobe.Models;

public partial class OrderDetail
{
    [Key]
    public int Orderdetailsid { get; set; }

    public int? Fkitemid { get; set; }

    public DateOnly? Quantity { get; set; }

    public decimal? Price { get; set; }

    public int? Fkorderid { get; set; }

    public virtual Item? Fkitem { get; set; }

    public virtual Order? Fkorder { get; set; }
}
