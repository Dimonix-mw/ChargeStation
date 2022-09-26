using PlatformService.MessageBroker.Publisher;
using Microsoft.AspNetCore.Mvc;
using PlatformService.MessageBroker.Settings;
using PlatformServiceBLL.Services.Interfaces;
using Microsoft.Extensions.Options;


namespace PlatformService.WebApi.Controllers
{
    public class PlatformServiceController : MainApiController
    {
        private readonly ISessionService _sessionService;
        private readonly IRabbitMqService _mqService;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly ILogger<PlatformServiceController> _logger;

        public PlatformServiceController(ISessionService sessionService, IRabbitMqService mqService,
            IOptions<RabbitMQSettings> rabbitMQSettings, ILogger<PlatformServiceController> logger)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _mqService = mqService ?? throw new ArgumentNullException(nameof(mqService));
            _rabbitMQSettings = rabbitMQSettings.Value;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> ChargeFinished([FromQuery] Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("Bad request, id is guid empty");
                return BadRequest(id);
            }
            var fillingDto = await _sessionService.GetFillingDtoAsync(id);
            //var updateFilling = _mapper.Map<FillingDto>(request);
            //await _sessionService.UpdateFillingAsync(updateFilling);
            _mqService.SendMessage(fillingDto, _rabbitMQSettings.CreateFillingMQ);
            _logger.LogInformation($"Send message to rabbitmq {_rabbitMQSettings.CreateFillingMQ}");
            return Ok();
        }

    }
}
