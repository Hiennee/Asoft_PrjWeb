using Microsoft.EntityFrameworkCore;
using TestPrj3.Models;

namespace TestPrj3.Services
{
    public class DepartmentService
    {
        private readonly Asoft2Context _context;

        public DepartmentService(Asoft2Context context)
        {
            _context = context;
        }

        public ICollection<Department> GetAllDepartments()
        {
            return _context.Departments.Include(d => d.Employees)
                   .Select(d => new Department
                   {
                       DepartmentId = d.DepartmentId,
                       DepartmentName = d.DepartmentName,
                       Employees = d.Employees.Select(e => new Employee
                       {
                           EmployeeId = e.EmployeeId,
                           EmployeeName = e.EmployeeName,
                           Phone = e.Phone,
                           PositionId = e.PositionId,
                           Position = new Position
                           {
                               PositionName = e.Position.PositionName,
                           }
                       }).ToList()
                   }).ToList();
        }

        public ICollection<Department> GetDepartmentsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            return _context.Departments.Where(d => d.DepartmentName.Contains(name))
                .Include(d => d.Employees)
                .Select(d => new Department
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName,
                    Employees = d.Employees.Select(e => new Employee
                    {
                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.EmployeeName,
                        Phone = e.Phone,
                        PositionId = e.PositionId,
                        Position = new Position
                        {
                            PositionName = e.Position.PositionName,
                        }
                    }).ToList()
                }).ToList();
        }

        public Department GetDepartmentById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            id = id.Replace("%2F", "/");
            return _context.Departments.Include(d => d.Employees)
                .Select(d => new Department
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName,
                    Employees = d.Employees.Select(e => new Employee
                    {
                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.EmployeeName,
                        Phone = e.Phone,
                        PositionId = e.PositionId,
                        Position = new Position
                        {
                            PositionName = e.Position.PositionName,
                        }
                    }).ToList()
                }).FirstOrDefault(d => d.DepartmentId.Equals(id));
        }

        public void AddDepartment(Department d)
        {
            if (string.IsNullOrEmpty(d.DepartmentId) || string.IsNullOrEmpty(d.DepartmentName))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var pkConfig = _context.PrimaryKeyConfigs.FirstOrDefault(pkc => pkc.TableName.Equals("Department"));

            d.DepartmentId = new PrimaryKeyConfigService(_context).GeneratePrimaryKey("Department");
            _context.Departments.Add(d);
            pkConfig.LastKey += 1;
            _context.PrimaryKeyConfigs.Update(pkConfig);
            _context.SaveChanges();
        }

        public void UpdateDepartment(string id, string name)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var department = _context.Departments.FirstOrDefault(d => d.DepartmentId.Equals(id));
            if (department == null)
            {
                throw new Exception("Department not found");
            }
            department.DepartmentName = name;
            _context.SaveChanges();
        }

        public void DeleteDepartment(string id)
        {
            id = id.Replace("%2F", "/");
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("INVALID_ARGUMENT");
            }
            var department = _context.Departments.FirstOrDefault(d => d.DepartmentId.Equals(id));
            if (department == null)
            {
                throw new Exception("NOT_FOUND");
            }
            if (department.Employees.Count > 0)
            {
                throw new Exception("RELATED");
            }
            _context.Departments.Remove(department);
            _context.SaveChanges();
        }
    }
}
