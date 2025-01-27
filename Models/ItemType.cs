using System;
using System.Collections.Generic;

namespace AI_Wardrobe.Models;

public partial class ItemType
{
    public int Itemtypeid { get; set; }

    public string? Itemtypedescription { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
