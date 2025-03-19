using System;
using System.Collections.Generic;

namespace AI_Wardrobe.Models;

public partial class OrderStatus
{
    public string Status { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
