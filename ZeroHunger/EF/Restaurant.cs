using System;
using System.Collections.Generic;

namespace ZeroHunger.EF;

public partial class Restaurant
{
    public int ResId { get; set; }

    public string ResName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; } = new List<Request>();
}
