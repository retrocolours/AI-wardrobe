using System;
using System.Collections.Generic;

namespace AI_Wardrobe.Models;

public partial class UserType
{
    public int Usertypeid { get; set; }

    public string? Usertype1 { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
