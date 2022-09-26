using MediatR;

namespace PaymentService.Application.CQRS.PumpModels.Commands.DeletePumpModel
{
    public class DeletePumpModelCommand : IRequest
    {
        public int Id { get; set; }
    }
}
