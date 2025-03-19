using System;
using System.Collections.Generic;

namespace AI_Wardrobe.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public DateOnly? Orderdate { get; set; }

    public string? Orderstatus { get; set; }

    public int Fkuserid { get; set; }

    public virtual RegisteredUser? Fkuser { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderStatus? OrderstatusNavigation { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
