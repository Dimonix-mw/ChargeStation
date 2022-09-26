using MediatR;
#pragma warning disable CS8618

namespace PaymentService.Application.CQRS.Filials.Commands.CreateFilial
{
    public class CreateFilialCommand : IRequest<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
