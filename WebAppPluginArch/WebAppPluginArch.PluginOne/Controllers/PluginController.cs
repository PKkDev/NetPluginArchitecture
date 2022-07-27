using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAppPluginArch.PluginOne.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PluginController : ControllerBase
    {
        private readonly ILogger<PluginController> _logger;
        private readonly PluginService _pluginService;

        public PluginController(
            ILogger<PluginController> logger, PluginService pluginService)
        {
            _logger = logger;
            _pluginService = pluginService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"{nameof(PluginController)} ok ${_pluginService.Test()}");
        }
    }
}
