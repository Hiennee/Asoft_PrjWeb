using System;
using System.Collections.Generic;

namespace TestPrj3.Models;

public partial class Position
{
    public string PositionId { get; set; } = null!;

    public string PositionName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
