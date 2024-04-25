using System;
using System.Collections.Generic;

namespace ZeroHunger.EF;

public partial class Food
{
    public int FoodId { get; set; }

    public string FoodName { get; set; } = null!;

    public float Quantity { get; set; }

    public int ReqId { get; set; }

    public virtual Request Req { get; set; } = null!;
}
