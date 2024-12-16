using System;
using System.Collections.Generic;

namespace AI_Wardrobe.Models;

public partial class ItemGender
{
    public int Itemgenderid { get; set; }

    public string? Itemgenderdescription { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
