using System;
using System.Collections.Generic;

namespace AI_Wardrobe.Models;

public partial class RegisteredUser
{
    public int Userid { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public int? Phone { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
