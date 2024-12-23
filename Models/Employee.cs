using System;
using System.Collections.Generic;

namespace TestPrj3.Models;

public partial class Employee
{
    public string EmployeeId { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? DepartmentId { get; set; }

    public string? PositionId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Department? Department { get; set; }

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public virtual Position? Position { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
}
