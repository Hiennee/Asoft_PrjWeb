using Microsoft.AspNetCore.Mvc;
using TestPrj3.Models;
using TestPrj3.Services;

namespace TestPrj3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public IActionResult GetAllProjects()
        {
            var projects = _projectService.GetAllProjects();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public IActionResult GetProjectById(string id)
        {
            var project = _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpPost]
        public IActionResult AddProject([FromBody] Project project)
        {
            _projectService.AddProject(project);
            return CreatedAtAction(nameof(GetProjectById), project);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProject(string id, [FromBody] Project project)
        {
            try
            {
                _projectService.UpdateProject(id, project.ProjectName, project.StartDate, project.EndDate, project.EmployeeId, project.Status);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProject(string id)
        {
            try
            {
                _projectService.DeleteProject(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
