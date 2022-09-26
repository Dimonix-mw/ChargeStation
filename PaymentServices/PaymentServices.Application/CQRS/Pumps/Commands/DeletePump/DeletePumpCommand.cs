using MediatR;

namespace PaymentService.Application.CQRS.Pumps.Commands.DeletePump
{
    public class DeletePumpCommand : IRequest
    {
        public int Id { get; set; }
    }
}
