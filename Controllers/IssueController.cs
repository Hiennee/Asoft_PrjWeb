using Microsoft.AspNetCore.Mvc;
using TestPrj3.Models;
using TestPrj3.Services;

namespace TestPrj3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IssueService _issueService;

        public IssueController(IssueService issueService)
        {
            _issueService = issueService;
        }

        // GET: api/issue
        [HttpGet]
        public IActionResult GetAllIssues()
        {
            var issues = _issueService.GetAllIssues();
            return Ok(issues);
        }

        // GET: api/issue/name
        [HttpGet("search")]
        public IActionResult GetIssuesByName([FromQuery] string name)
        {
            try
            {
                var issues = _issueService.GetIssuesByName(name);
                return Ok(issues);
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid name parameter.");
            }
        }

        // GET: api/issue/{id}
        [HttpGet("{id}")]
        public IActionResult GetIssueById(string id)
        {
            var issue = _issueService.GetIssueById(id);
            if (issue == null)
            {
                return NotFound("Issue not found.");
            }
            return Ok(issue);
        }

        // POST: api/issue
        [HttpPost]
        public IActionResult AddIssue([FromBody] IssueDTO issue)
        {
            try
            {
                _issueService.AddIssue(issue);
                return CreatedAtAction(nameof(GetIssueById), issue);
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid issue data.");
            }
        }

        // PUT: api/issue/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateIssue(string id, [FromBody] IssueDTO issue)
        {
            try
            {
                _issueService.UpdateIssue(id, issue.IssueName, issue.IssueType, issue.EmployeeId, issue.TaskId, issue.ProjectId, issue.Description, issue.ReportedDate, issue.Deadline, issue.Status);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound("Issue not found.");
            }
        }

        // DELETE: api/issue/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteIssue(string id)
        {
            try
            {
                _issueService.DeleteIssue(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound("Issue not found.");
            }
        }
    }
}

public class IssueDTO
{
    public string IssueName { get; set; }
    public string IssueType { get; set; }
    public string Description { get; set; }
    public DateTime ReportedDate { get; set; }
    public DateTime Deadline { get; set; }
    public string Status { get; set; }
    public string? EmployeeId { get; set; }
    public string? ProjectId { get; set; }
    public string? TaskId { get; set; }
}
