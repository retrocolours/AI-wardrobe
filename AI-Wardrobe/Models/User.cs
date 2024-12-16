using System;
using System.Collections.Generic;

namespace AI_Wardrobe.Models;

public partial class User
{
    public int Userid { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? Fkusertypeid { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual UserType? Fkusertype { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
