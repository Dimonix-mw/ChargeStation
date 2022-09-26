using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PaymentService.Application.CQRS.Fillings.Commands.CreateFilling;
using PaymentService.Application.CQRS.Fillings.Commands.UpdateFilling;
using PaymentService.Application.CQRS.Prices.Queries.GetPriceList;
using PaymentService.Application.CQRS.Prices.Queries.GetPriceListByPumpId;
using PaymentService.MessageBrocker.Consumer.Models;
using PaymentService.MessageBroker.Common.Settings;
using PaymentService.MessageBroker.Common.Publisher;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
#pragma warning disable CS8602
#pragma warning disable CS8604

namespace PaymentService.MessageBrocker.Consumer.Services
{
    public class ListenerRabbitMQService : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<ListenerRabbitMQService> _logger;
        private readonly IRabbitMqService _mqService;

        public ListenerRabbitMQService(IOptions<RabbitMQSettings> rabbitMQSettings, 
            IMediator mediator, IMapper mapper, 
            ILogger<ListenerRabbitMQService> logger,
            IRabbitMqService mqService)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
            _rabbitMQSettings = rabbitMQSettings.Value;
            _mqService = mqService;
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQSettings.Hostname,
                UserName = _rabbitMQSettings.UserName,
                Password = _rabbitMQSettings.Password,
                VirtualHost = _rabbitMQSettings.VirtualHost
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: _rabbitMQSettings.CreateFillingMQ,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                try
                {
                    var createFillingDto = JsonConvert.DeserializeObject<CreateFillingDto>(content);
                    
                    var fillingId = await CreateFillingAsync(createFillingDto, cancellationToken);

                    var priceList = await GetPriceListByPumpIdAsync(pumpId: createFillingDto.PumpId, cancellationToken);

                    createFillingDto.TotalMoneyAmount = createFillingDto.Minutes * priceList.Prices.FirstOrDefault().Cost;

                    await UpdateFillingAsync(createFillingDto, cancellationToken);
                        
                    _channel.BasicAck(ea.DeliveryTag, false);

                    //var updateFillingResponceDto = new UpdateFillingResponceDto { Id = createFillingDto.Id, TotalMoneyAmount = createFillingDto.TotalMoneyAmount };
                    _logger.LogDebug($"Send to rabbitmq {_rabbitMQSettings.UpdateFillingMQ} data createFillingDto " +
                        $"id=> {createFillingDto.Id}," +
                        $"PumpId=> {createFillingDto.PumpId}," +
                        $"Minutes=> { createFillingDto.Minutes}," +
                        $"TotalMoneyAmount => {createFillingDto.TotalMoneyAmount}");
                    _mqService.SendMessage(createFillingDto, _rabbitMQSettings.UpdateFillingMQ);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"CreateFillingMQ Received Error");
                }

            };
            try
            {
                _channel.BasicConsume(_rabbitMQSettings.CreateFillingMQ, false, consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CreateFillingMQ BasicConsume Error");
            }

            return Task.CompletedTask;
        }

        protected async Task<PriceList> GetPriceListByPumpIdAsync(int pumpId, CancellationToken cancellationToken)
        {
            var getPriceListByIdDto = new GetPriceListByPumpIdDto { PumpId = pumpId };

            var queryGetPriceListByPumpId = _mapper.Map<GetPriceListByPumpIdQuery>(getPriceListByIdDto);

            return await _mediator.Send(queryGetPriceListByPumpId, cancellationToken);
        }

        protected async Task<int> CreateFillingAsync(CreateFillingDto createFillingDto, CancellationToken cancellationToken)
        {
            var commandCreateFilling = _mapper.Map<CreateFillingCommand>(createFillingDto);

            return await _mediator.Send(commandCreateFilling, cancellationToken);
        }

        protected async Task UpdateFillingAsync(CreateFillingDto createFillingDto, CancellationToken cancellationToken)
        {
            var UpdateFillingDto = new UpdateFillingDto
            {
                Id = createFillingDto.Id,
                PumpId = createFillingDto.PumpId,
                PromotionId = createFillingDto.PromotionId,
                BonusAmount = createFillingDto.BonusAmount,
                BonusCalculateRuleId = createFillingDto.BonusCalculateRuleId,
                Minutes = createFillingDto.Minutes,
                PromotionAmount = createFillingDto.PromotionAmount,
                TotalMoneyAmount = createFillingDto.TotalMoneyAmount
            };

            var commandUpdateFilling = _mapper.Map<UpdateFillingCommand>(UpdateFillingDto);

            await _mediator.Send(commandUpdateFilling, cancellationToken);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
