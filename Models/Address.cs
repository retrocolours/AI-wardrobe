using System;
using System.Collections.Generic;

namespace AI_Wardrobe.Models;

public partial class Address
{
    public int Addressid { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    public string? Postalcode { get; set; }

    public string? Country { get; set; }

    public int? Fkuserid { get; set; }

    public virtual RegisteredUser? Fkuser { get; set; }
}
