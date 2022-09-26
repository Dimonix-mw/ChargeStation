using MediatR;
#pragma warning disable CS8618

namespace PaymentService.Application.CQRS.Filials.Commands.UpdateFilial
{
    public class UpdateFilialCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
