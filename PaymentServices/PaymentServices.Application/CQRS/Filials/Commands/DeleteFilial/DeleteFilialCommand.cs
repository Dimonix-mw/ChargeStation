using MediatR;

namespace PaymentService.Application.CQRS.Filials.Commands.DeleteFilial
{
    public class DeleteFilialCommand : IRequest
    {
        public int Id { get; set; }
    }
}
