using System;
using System.Collections.Generic;

namespace AI_Wardrobe.Models;

public partial class Item
{
    public int Itemid { get; set; }

    public string? Itemtype { get; set; }

    public string? ItemName { get; set; }

    public string? Itemdescription { get; set; }

    public decimal? Itemprice { get; set; }

    public string? Imageurl { get; set; }

    public int? Fkitemgenderid { get; set; }

    public int? Fksizeid { get; set; }

    public int? Fktypeid { get; set; }

    public virtual ItemGender? Fkitemgender { get; set; }

    public virtual Size? Fksize { get; set; }

    public virtual ItemType? Fktype { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
