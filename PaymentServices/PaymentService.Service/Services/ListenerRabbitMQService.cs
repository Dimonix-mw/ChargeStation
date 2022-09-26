using AutoMapper;
using MediatR;
using PaymentService.Application.CQRS.Fillings.Commands.CreateFilling;
using PaymentService.MessageBrocker.Consumer.Models;

namespace PaymentService.MessageBrocker.Consumer.Services
{
    public class ListenerRabbitMQService : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        
        public ListenerRabbitMQService(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var createFillingDto = new CreateFillingDto();

            createFillingDto.Id = 1;
            createFillingDto.PumpId = 1;
            createFillingDto.Minutes = 50;

            var command = _mapper.Map<CreateFillingCommand>(createFillingDto);

            _mediator.Send(command);

            return Task.CompletedTask;
        }
    }
}
