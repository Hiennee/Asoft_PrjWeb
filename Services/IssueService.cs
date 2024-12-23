using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using TestPrj3.Models;

namespace TestPrj3.Services
{
    public class IssueService
    {
        private readonly Asoft2Context _context;

        public IssueService(Asoft2Context context)
        {
            _context = context;
        }

        public ICollection<Issue> GetAllIssues()
        {
            return _context.Issues.Include(i => i.Task)
                                  .Include(i => i.Project)
                                  .Include(i => i.Employee)
                                  .Select(i => new Issue
                                  {
                                      IssueId = i.IssueId,
                                      IssueName = i.IssueName,
                                      IssueType = i.IssueType,
                                      Description = i.Description,
                                      ReportedDate = i.ReportedDate,
                                      Deadline = i.Deadline,
                                      Status = i.Status,
                                      TaskId = i.TaskId,
                                      ProjectId = i.ProjectId,
                                      EmployeeId = i.EmployeeId,
                                      Task = i.Task.TaskId == null ? null :
                                      new Tasks
                                      {
                                          TaskId = i.Task.TaskId,
                                          TaskName = i.Task.TaskName,
                                          TaskType = i.Task.TaskType,
                                          StartDate = i.Task.StartDate,
                                          EndDate = i.Task.EndDate,
                                          Status = i.Task.Status,
                                      },
                                      Project = i.Project.ProjectId == null ? null:
                                      new Project
                                      {
                                          ProjectId = i.Project.ProjectId,
                                          ProjectName = i.Project.ProjectName,
                                          StartDate = i.Project.StartDate,
                                          EndDate = i.Project.EndDate,
                                          Status = i.Project.Status,
                                      },
                                      Employee = i.Employee.EmployeeId == null ? null :
                                      new Employee
                                      {
                                          EmployeeId = i.Employee.EmployeeId,
                                          EmployeeName = i.Employee.EmployeeName,
                                          Phone = i.Employee.Phone,
                                          Status = i.Employee.Status,
                                      }
                                  }).ToList();
        }

        public ICollection<Issue> GetIssuesByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            return _context.Issues.Where(i => i.IssueName.Contains(name))
                                  .Include(i => i.Task)
                                  .Include(i => i.Project)
                                  .Include(i => i.Employee)
                                  .Select(i => new Issue
                                  {
                                      IssueId = i.IssueId,
                                      IssueName = i.IssueName,
                                      IssueType = i.IssueType,
                                      Description = i.Description,
                                      ReportedDate = i.ReportedDate,
                                      Deadline = i.Deadline,
                                      Status = i.Status,
                                      TaskId = i.TaskId,
                                      ProjectId = i.ProjectId,
                                      EmployeeId = i.EmployeeId,
                                      Task = new Tasks
                                      {
                                          TaskId = i.Task.TaskId,
                                          TaskName = i.Task.TaskName,
                                          TaskType = i.Task.TaskType,
                                          StartDate = i.Task.StartDate,
                                          EndDate = i.Task.EndDate,
                                          Status = i.Task.Status,
                                      },
                                      Project = new Project
                                      {
                                          ProjectId = i.Project.ProjectId,
                                          ProjectName = i.Project.ProjectName,
                                          StartDate = i.Project.StartDate,
                                          EndDate = i.Project.EndDate,
                                          Status = i.Project.Status,
                                      },
                                      Employee = new Employee
                                      {
                                          EmployeeId = i.Employee.EmployeeId,
                                          EmployeeName = i.Employee.EmployeeName,
                                          Phone = i.Employee.Phone,
                                          Status = i.Employee.Status,
                                      }
                                  }).ToList();
        }

        public Issue GetIssueById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            return _context.Issues.Include(i => i.Task)
                                  .Include(i => i.Project)
                                  .Include(i => i.Employee)
                                  .Select(i => new Issue
                                  {
                                      IssueId = i.IssueId,
                                      IssueName = i.IssueName,
                                      IssueType = i.IssueType,
                                      Description = i.Description,
                                      ReportedDate = i.ReportedDate,
                                      Deadline = i.Deadline,
                                      Status = i.Status,
                                      TaskId = i.TaskId,
                                      ProjectId = i.ProjectId,
                                      EmployeeId = i.EmployeeId,
                                      Task = i.Task.TaskId == null ? null :
                                      new Tasks
                                      {
                                          TaskId = i.Task.TaskId,
                                          TaskName = i.Task.TaskName,
                                          TaskType = i.Task.TaskType,
                                          StartDate = i.Task.StartDate,
                                          EndDate = i.Task.EndDate,
                                          Status = i.Task.Status,
                                      },
                                      Project = i.Project.ProjectId == null ? null :
                                      new Project
                                      {
                                          ProjectId = i.Project.ProjectId,
                                          ProjectName = i.Project.ProjectName,
                                          StartDate = i.Project.StartDate,
                                          EndDate = i.Project.EndDate,
                                          Status = i.Project.Status,
                                      },
                                      Employee = i.Employee.EmployeeId == null ? null :
                                      new Employee
                                      {
                                          EmployeeId = i.Employee.EmployeeId,
                                          EmployeeName = i.Employee.EmployeeName,
                                          Phone = i.Employee.Phone,
                                          Status = i.Employee.Status,
                                      }
                                  }).FirstOrDefault(i => i.IssueId.Equals(id));
        }

        public void AddIssue(IssueDTO issue)
        {
            if (string.IsNullOrEmpty(issue.IssueName) ||
                string.IsNullOrEmpty(issue.IssueType) || string.IsNullOrEmpty(issue.Description) ||
                string.IsNullOrEmpty(issue.Status) || (string.IsNullOrEmpty(issue.EmployeeId) &&
                string.IsNullOrEmpty(issue.ProjectId) && string.IsNullOrEmpty(issue.TaskId)))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var pkConfig = _context.PrimaryKeyConfigs.FirstOrDefault(pkc => pkc.TableName.Equals("Issue"));

            string issueId = new PrimaryKeyConfigService(_context).GeneratePrimaryKey("Issue");
            issue.EmployeeId = issue.EmployeeId == null ? null : issue.EmployeeId.Replace("%2F", "/");
            issue.ProjectId = issue.ProjectId == null ? null : issue.ProjectId.Replace("%2F", "/");
            issue.TaskId = issue.TaskId == null ? null : issue.TaskId.Replace("%2F", "/");

            Issue i = new Issue
            {
                IssueId = issueId,
                IssueName = issue.IssueName,
                IssueType = issue.IssueType,
                Description = issue.Description,
                ReportedDate = issue.ReportedDate,
                Deadline = issue.Deadline,
                Status = issue.Status,
                TaskId = issue.TaskId,
                ProjectId = issue.ProjectId,
                EmployeeId = issue.EmployeeId,
            };
            _context.Issues.Add(i);
            pkConfig.LastKey += 1;
            _context.PrimaryKeyConfigs.Update(pkConfig);
            _context.SaveChanges();
        }

        public void UpdateIssue(string id, string name, string type, string? employeeId, string? taskId, string? projectId, string description, DateTime reportedDate, DateTime deadline, string status)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) ||
                string.IsNullOrEmpty(description) || string.IsNullOrEmpty(status))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            id = id.Replace("%2F", "/");
            employeeId = employeeId.Replace("%2F", "/");
            taskId = taskId == null || taskId.Equals("") ? null : taskId.Replace("%2F", "/");
            projectId = projectId == null || projectId.Equals("") ? null : projectId.Replace("%2F", "/");

            var issue = _context.Issues.FirstOrDefault(i => i.IssueId.Equals(id));
            if (issue == null)
            {
                throw new Exception("NOT_FOUND");
            }
            issue.IssueName = name;
            issue.IssueType = type;
            issue.TaskId = taskId;
            issue.ProjectId = projectId;
            issue.Description = description;
            issue.ReportedDate = reportedDate;
            issue.Deadline = deadline;
            issue.Status = status;
            _context.SaveChanges();
        }

        public void DeleteIssue(string id)
        {
            id = id.Replace("%2F", "/");
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var issue = _context.Issues.FirstOrDefault(i => i.IssueId.Equals(id));
            if (issue == null)
            {
                throw new Exception("NOT_FOUND");
            }
            _context.Issues.Remove(issue);
            _context.SaveChanges();
        }
    }
}
