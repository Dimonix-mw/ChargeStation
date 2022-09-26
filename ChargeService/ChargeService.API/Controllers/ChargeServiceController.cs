using ChargeService.API.Controllers.Common;
using ChargeService.BLL.Dtos;
using ChargeService.BLL.Services.Interfaces;
using ChargeService.MessageBroker.Publisher;
using ChargeService.MessageBroker.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ChargeService.API.Controllers
{
    /// <summary>
    /// Контроллер зарядки
    /// </summary>
    public class ChargeServiceController : MainApiController
    {
        private readonly ISessionService _sessionService;
        private readonly IRabbitMqService _mqService;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly ILogger<ChargeServiceController> _logger;

        public ChargeServiceController(ISessionService sessionService, IRabbitMqService mqService,
            ILogger<ChargeServiceController> logger,
            IOptions<RabbitMQSettings> rabbitMQSettings)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _mqService = mqService ?? throw new ArgumentNullException(nameof(mqService));
            _rabbitMQSettings = rabbitMQSettings.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Проверка статуса зарядки
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> CheckStatus([FromQuery] CheckPumpStatusRequestDto request)
        {
            if (request == null || request.RequestId == Guid.Empty)
                return BadRequest(request);
            return Ok(await _sessionService.CheckStatusAsync(request));
        }
        /// <summary>
        /// Запрос на зарядку автомобиля 
        /// от мобильного устройства
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> InsertRequest([FromBody] InsertPumpRequestDto request)
        {
            if (request == null || request.RequestId == Guid.Empty)
                return BadRequest(request);
            _logger.LogInformation($"InsertRequest, RequestId = {request.RequestId}," +
                $"UserId = {request.UserId}");
            var responseDto = await _sessionService.InsertAsync(request);
            var mqMessage = await _sessionService.GetStartChargeMQRequestAsync(request.RequestId);
            _mqService.SendMessage(mqMessage, _rabbitMQSettings.StartChargeMQ);
            return Ok(responseDto);
        }
    }
}
