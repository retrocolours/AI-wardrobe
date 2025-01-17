using System;
using System.Collections.Generic;

namespace AI_Wardrobe.Models;

public partial class Size
{
    public int Sizeid { get; set; }

    public string? Sizedescription { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
