using Microsoft.AspNetCore.Mvc;
using TestPrj3.Models;
using TestPrj3.Services;

namespace TestPrj3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TasksService _taskService;

        public TaskController(TasksService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult GetAllTasks()
        {
            var tasks = _taskService.GetAllTasks();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public IActionResult GetTaskById(string id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public IActionResult AddTask([FromBody] Tasks task)
        {
            _taskService.AddTask(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.TaskId }, task);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(string id, [FromBody] Tasks task)
        {
            try
            {
                _taskService.UpdateTask(id, task.TaskName, task.TaskType, task.ProjectId, task.EmployeeId, task.StartDate, task.EndDate, task.Status);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(string id)
        {
            try
            {
                _taskService.DeleteTask(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
