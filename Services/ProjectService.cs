using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestPrj3.Models;

namespace TestPrj3.Services
{
    public class ProjectService
    {
        private readonly Asoft2Context _context;

        public ProjectService(Asoft2Context context)
        {
            _context = context;
        }

        public ICollection<Project> GetAllProjects()
        {
            return _context.Projects.Include(p => p.Employee)
            .Select(p => new Project
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                EmployeeId = p.EmployeeId,
                Status = p.Status,
                Employee = new Employee
                {
                    EmployeeId = p.EmployeeId,
                    EmployeeName = p.Employee.EmployeeName,
                    Phone = p.Employee.Phone,
                    Status = p.Employee.Status,
                }
            }).ToList();
        }

        public ICollection<Project> GetProjectsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            return _context.Projects
                    .Where(p => p.ProjectName.Contains(name))
                    .Include(p => p.Employee)
                    .Select(p => new Project
                    {
                        ProjectId = p.ProjectId,
                        ProjectName = p.ProjectName,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        EmployeeId = p.EmployeeId,
                        Status = p.Status,
                        Employee = new Employee
                        {
                            EmployeeId = p.EmployeeId,
                            EmployeeName = p.Employee.EmployeeName,
                            Phone = p.Employee.Phone,
                            Status = p.Employee.Status,
                        }
                    }).ToList();
        }

        public Project GetProjectById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            return _context.Projects.Include(p => p.Employee)
            .Select(p => new Project
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                EmployeeId = p.EmployeeId,
                Status = p.Status,
                Employee = new Employee
                {
                    EmployeeId = p.EmployeeId,
                    EmployeeName = p.Employee.EmployeeName,
                    Phone = p.Employee.Phone,
                    Status = p.Employee.Status,
                }
            }).FirstOrDefault(p => p.ProjectId.Equals(id));
        }

        public void AddProject(Project project)
        {
            if (string.IsNullOrEmpty(project.ProjectId) || string.IsNullOrEmpty(project.ProjectName) ||
                string.IsNullOrEmpty(project.Status))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var pkConfig = _context.PrimaryKeyConfigs.FirstOrDefault(pkc => pkc.TableName.Equals("Project"));

            project.ProjectId = new PrimaryKeyConfigService(_context).GeneratePrimaryKey("Project");
            _context.Projects.Add(project);
            pkConfig.LastKey += 1;
            _context.PrimaryKeyConfigs.Update(pkConfig);
            _context.SaveChanges();
        }

        public void UpdateProject(string id, string name, DateTime startDate, DateTime endDate, string? employeeId, string status)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(status))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            id = id.Replace("%2F", "/");
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId.Equals(id));
            if (project == null)
            {
                throw new Exception("NOT_FOUND");
            }
            project.ProjectName = name;
            project.StartDate = startDate;
            project.EndDate = endDate;
            project.EmployeeId = employeeId;
            project.Status = status;
            _context.SaveChanges();
        }

        public void DeleteProject(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            id = id.Replace("%2F", "/");
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId.Equals(id));
            if (project == null)
            {
                throw new Exception("NOT_FOUND");
            }
            if (project.Issues.Count > 0 || project.Tasks.Count > 0)
            {
                throw new Exception("RELATED");
            }
            _context.Projects.Remove(project);
            _context.SaveChanges();
        }
    }
}
