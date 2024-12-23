using Microsoft.AspNetCore.Mvc;
using TestPrj3.Models;
using TestPrj3.Services;

namespace TestPrj3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrimaryKeyConfigController : ControllerBase
    {
        private readonly PrimaryKeyConfigService _primaryKeyConfigService;

        public PrimaryKeyConfigController(PrimaryKeyConfigService primaryKeyConfigService)
        {
            _primaryKeyConfigService = primaryKeyConfigService;
        }

        [HttpGet]
        public IActionResult GetAllConfigs()
        {
            var configs = _primaryKeyConfigService.GetAllConfigs();
            return Ok();
        }
    }
}
