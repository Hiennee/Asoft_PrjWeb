using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TestPrj3.Models;

namespace TestPrj3.Services
{
    public class TasksService
    {
        private readonly Asoft2Context _context;

        public TasksService(Asoft2Context context)
        {
            _context = context;
        }

        public ICollection<Tasks> GetAllTasks()
        {
            return _context.Tasks.Include(t => t.Project).Include(t => t.Employee)
            .Select(t => new Tasks
            {
                TaskId = t.TaskId,
                TaskName = t.TaskName,
                TaskType = t.TaskType,
                ProjectId = t.ProjectId,
                EmployeeId = t.EmployeeId,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Status = t.Status,
                Project = t.Project == null ? null : 
                new Project
                {
                    ProjectId = t.Project.ProjectId,
                    ProjectName = t.Project.ProjectName,
                },
                Employee = t.Employee == null ? null :
                new Employee
                {
                    EmployeeId = t.Employee.EmployeeId,
                    EmployeeName = t.Employee.EmployeeName,
                }
            }).ToList();
        }

        public ICollection<Tasks> GetTasksByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            return _context.Tasks.Where(t => t.TaskName.Contains(name))
                .Include(t => t.Project)
                .Include(t => t.Employee)
                .Select(t => new Tasks
                {
                    TaskId = t.TaskId,
                    TaskName = t.TaskName,
                    TaskType = t.TaskType,
                    ProjectId = t.ProjectId,
                    EmployeeId = t.EmployeeId,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    Status = t.Status,
                    Project = t.Project == null ? null :
                new Project
                {
                    ProjectId = t.Project.ProjectId,
                    ProjectName = t.Project.ProjectName,
                },
                    Employee = t.Employee == null ? null :
                new Employee
                {
                    EmployeeId = t.Employee.EmployeeId,
                    EmployeeName = t.Employee.EmployeeName,
                }
                }).ToList();
        }

        public Tasks GetTaskById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            return _context.Tasks.Include(t => t.Project).Include(t => t.Employee)
            .Select(t => new Tasks
            {
                TaskId = t.TaskId,
                TaskName = t.TaskName,
                TaskType = t.TaskType,
                ProjectId = t.ProjectId,
                EmployeeId = t.EmployeeId,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Status = t.Status,
                Project = t.Project == null ? null :
                new Project
                {
                    ProjectId = t.Project.ProjectId,
                    ProjectName = t.Project.ProjectName,
                },
                Employee = t.Employee == null ? null :
                new Employee
                {
                    EmployeeId = t.Employee.EmployeeId,
                    EmployeeName = t.Employee.EmployeeName,
                }
            }).FirstOrDefault(t => t.TaskId.Equals(id));
        }

        public void AddTask(Tasks task)
        {
            if (string.IsNullOrEmpty(task.TaskId) || string.IsNullOrEmpty(task.TaskName) ||
                string.IsNullOrEmpty(task.TaskType) || string.IsNullOrEmpty(task.Status))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var pkConfig = _context.PrimaryKeyConfigs.FirstOrDefault(pkc => pkc.TableName.Equals("Task"));

            task.TaskId = new PrimaryKeyConfigService(_context).GeneratePrimaryKey("Task");
            task.EmployeeId = task.EmployeeId == null || task.EmployeeId.Equals("") ? null : task.EmployeeId.Replace("%2F", "/");
            _context.Tasks.Add(task);
            pkConfig.LastKey += 1;
            _context.PrimaryKeyConfigs.Update(pkConfig);
            _context.SaveChanges();
        }

        public void UpdateTask(string id, string name, string type, string? projectId, string? employeeId, DateTime startDate, DateTime endDate, string status)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) ||
                string.IsNullOrEmpty(status))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            id = id.Replace("%2F", "/");
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId.Equals(id));
            if (task == null)
            {
                throw new Exception("NOT_FOUND");
            }
            
            projectId = projectId == null || projectId == "" ? null : projectId.Replace("%2F", "/");
            employeeId = employeeId == null || employeeId == "" ? null : employeeId.Replace("%2F", "/");

            task.TaskName = name;
            task.TaskType = type;
            task.ProjectId = projectId;
            task.EmployeeId = employeeId;
            task.StartDate = startDate;
            task.EndDate = endDate;
            task.Status = status;
            _context.SaveChanges();
        }

        public void DeleteTask(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            id = id.Replace("%2F", "/");
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId.Equals(id));
            if (task == null)
            {
                throw new Exception("NOT_FOUND");
            }
            if (task.Issues.Any())
            {
                throw new Exception("RELATED");
            }
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }
    }
}
