using System;
using System.Collections.Generic;

namespace TestPrj3.Models;

public partial class Tasks
{
    public string TaskId { get; set; } = null!;

    public string TaskName { get; set; } = null!;

    public string TaskType { get; set; } = null!;

    public string? ProjectId { get; set; }

    public string? EmployeeId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public virtual Project? Project { get; set; }
}
