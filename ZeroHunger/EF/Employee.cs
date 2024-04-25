using System;
using System.Collections.Generic;

namespace ZeroHunger.EF;

public partial class Employee
{
    public int EmpId { get; set; }

    public string EmpName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public float Salary { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; } = new List<Request>();
}
