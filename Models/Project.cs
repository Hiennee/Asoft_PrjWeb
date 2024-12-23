using System;
using System.Collections.Generic;

namespace TestPrj3.Models;

public partial class Project
{
    public string ProjectId { get; set; } = null!;

    public string ProjectName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? EmployeeId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public virtual ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
}
