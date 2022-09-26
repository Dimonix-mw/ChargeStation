using HardwareEmulator.Models;
using HardwareEmulator.Services;
using Microsoft.AspNetCore.Mvc;

namespace HardwareEmulator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmulatorController : ControllerBase
    {
        private static Guid requestId;
        private readonly EmulatorService _emulatorService;
        private readonly ILogger<EmulatorController> _logger;

        public EmulatorController(EmulatorService emulatorService, ILogger<EmulatorController> logger)
        {
            _emulatorService = emulatorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> StartCharge([FromQuery]StartHardwareCharge request)
        {
            if (request.RequestId != requestId)
            {
                requestId = request.RequestId;
                _logger.LogDebug("StartCharge emulator");
                await _emulatorService.StartCharge(request);
            } 
            return Ok();
        }
    }
}
