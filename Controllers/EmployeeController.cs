using Microsoft.AspNetCore.Mvc;
using TestPrj3.Models;
using TestPrj3.Services;

namespace TestPrj3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(string id)
        {
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult AddEmployee([FromBody] Employee employee)
        {
            try
            {
                _employeeService.AddEmployee(employee);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeId }, employee);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(string id, [FromBody] Employee employee)
        {
            try
            {
                _employeeService.UpdateEmployee(id, employee.EmployeeName, employee.Phone, employee.DepartmentId,
                                                employee.PositionId, employee.Status);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(string id)
        {
            try
            {
                _employeeService.DeleteEmployee(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
