using System;
using System.Collections.Generic;

namespace TestPrj3.Models;

public partial class Issue
{
    public string IssueId { get; set; } = null!;

    public string IssueName { get; set; } = null!;

    public string IssueType { get; set; } = null!;

    public string? TaskId { get; set; }

    public string? ProjectId { get; set; }

    public string Description { get; set; } = null!;

    public string EmployeeId { get; set; } = null!;

    public DateTime ReportedDate { get; set; }

    public DateTime Deadline { get; set; }

    public string Status { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Project? Project { get; set; }

    public virtual Tasks? Task { get; set; }
}
