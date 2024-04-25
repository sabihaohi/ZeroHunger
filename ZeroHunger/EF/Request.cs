using System;
using System.Collections.Generic;

namespace ZeroHunger.EF;

public partial class Request
{
    public int ReqId { get; set; }

    public int ResId { get; set; }

    public DateTime ReqTime { get; set; }

    public DateTime PreserveTime { get; set; }

    public int EmpId { get; set; }

    public DateTime? CollectTime { get; set; }

    public DateTime? CompleteTime { get; set; }

    public string Status { get; set; } = null!;

    public float TotalCost { get; set; }

    public virtual Employee Emp { get; set; } = null!;

    public virtual ICollection<Food> Foods { get; } = new List<Food>();

    public virtual Restaurant Res { get; set; } = null!;
}
