using Microsoft.AspNetCore.Mvc;
using TestPrj3.Models;
using TestPrj3.Services;

namespace TestPrj3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _departmentService;

        
        public DepartmentController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        //[HttpGet("asd")]
        //public IActionResult anything()
        //{
        //    return Ok(PrimaryKeyConfigService.GeneratePrimaryKey("Department"));
        //}

        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            var departments = _departmentService.GetAllDepartments();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public IActionResult GetDepartmentById(string id)
        {
            var department = _departmentService.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpPost]
        public IActionResult AddDepartment([FromBody] Department department)
        {
            _departmentService.AddDepartment(department);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.DepartmentId }, department);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(string id, [FromQuery] string name)
        {
            try
            {
                id = id.Replace("%2F", "/");
                _departmentService.UpdateDepartment(id, name);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(string id)
        {
            try
            {
                _departmentService.DeleteDepartment(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
