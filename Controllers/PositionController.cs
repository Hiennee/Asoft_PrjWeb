using Microsoft.AspNetCore.Mvc;
using TestPrj3.Models;
using TestPrj3.Services;

namespace TestPrj3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly PositionService _positionService;

        public PositionController(PositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet]
        public IActionResult GetAllPositions()
        {
            var positions = _positionService.GetAllPositions();
            return Ok(positions);
        }

        [HttpGet("{id}")]
        public IActionResult GetPositionById(string id)
        {
            var position = _positionService.GetPositionById(id);
            if (position == null)
            {
                return NotFound();
            }
            return Ok(position);
        }

        [HttpPost]
        public IActionResult AddPosition([FromBody] Position position)
        {
            
            _positionService.AddPosition(position);
            return CreatedAtAction(nameof(GetPositionById), new { id = position.PositionId }, position);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePosition(string id, [FromBody] Position position)
        {
            try
            {
                _positionService.UpdatePosition(id, position.PositionName, position.Description);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePosition(string id)
        {
            try
            {
                _positionService.DeletePosition(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
