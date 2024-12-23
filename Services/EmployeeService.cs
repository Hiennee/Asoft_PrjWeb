using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestPrj3.Models;

namespace TestPrj3.Services
{
    public class EmployeeService
    {
        private readonly Asoft2Context _context;

        public EmployeeService(Asoft2Context context)
        {
            _context = context;
        }

        public ICollection<Employee> GetAllEmployees()
        {
            return _context.Employees.Include(e => e.Department)
                                     .Include(e => e.Position)
                                     .Select(e => new Employee
                                     {
                                         EmployeeId = e.EmployeeId,
                                         EmployeeName = e.EmployeeName,
                                         Phone = e.Phone,
                                         Department = new Department
                                         {
                                             DepartmentId = e.Department.DepartmentId,
                                             DepartmentName = e.Department.DepartmentName,
                                         },
                                         Position = new Position
                                         {
                                             PositionId = e.Position.PositionId,
                                             PositionName = e.Position.PositionName,
                                         },
                                         Status = e.Status,
                                     }).ToList();
        }

        public ICollection<Employee> GetEmployeesByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            return _context.Employees.Include(e => e.Department).Include(e => e.Position)
                   .Where(e => e.EmployeeName.Contains(name))
                   .Select(e => new Employee
                   {
                       EmployeeId = e.EmployeeId,
                       EmployeeName = e.EmployeeName,
                       Phone = e.Phone,
                       Department = new Department
                       {
                           DepartmentId = e.Department.DepartmentId,
                           DepartmentName = e.Department.DepartmentName,
                       },
                       Position = new Position
                       {
                           PositionId = e.Position.PositionId,
                           PositionName = e.Position.PositionName,
                       },
                       Status = e.Status,
                   }).ToList();
        }

        public Employee GetEmployeeById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            return _context.Employees.FirstOrDefault(e => e.EmployeeId.Equals(id));
        }

        public void AddEmployee(Employee e)
        {
            if (string.IsNullOrEmpty(e.EmployeeId) || string.IsNullOrEmpty(e.EmployeeName) || string.IsNullOrEmpty(e.Phone) || string.IsNullOrEmpty(e.Status))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var pkConfig = _context.PrimaryKeyConfigs.FirstOrDefault(pkc => pkc.TableName.Equals("Employee"));

            e.EmployeeId = new PrimaryKeyConfigService(_context).GeneratePrimaryKey("Employee");
            e.PositionId = e.PositionId.Replace("%2F", "/");
            e.DepartmentId = e.DepartmentId.Replace("%2F", "/");
            _context.Employees.Add(e);
            pkConfig.LastKey += 1;
            _context.PrimaryKeyConfigs.Update(pkConfig);
            _context.SaveChanges();
        }

        public void UpdateEmployee(string id, string name, string phone, string? departmentId, string? positionId, string status)
        {
            id = id.Replace("%2F", "/");
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(status))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId.Equals(id));
            if (employee == null)
            {
                throw new Exception("NOT_FOUND");
            }

            employee.EmployeeName = name;
            employee.Phone = phone;
            employee.DepartmentId = departmentId.Replace("%2F", "/");
            employee.PositionId = positionId.Replace("%2F", "/");
            employee.Status = status;
            _context.SaveChanges();
        }

        public void DeleteEmployee(string id)
        {
            id = id.Replace("%2F", "/");
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId.Equals(id));
            if (employee == null)
            {
                throw new Exception("NOT_FOUND");
            }
            if (employee.Issues.Count > 0 || employee.Projects.Count > 0 || employee.Tasks.Count > 0)
            {
                throw new Exception("RELATED");
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}
