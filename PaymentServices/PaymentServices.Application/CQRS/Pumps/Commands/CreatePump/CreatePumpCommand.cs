using MediatR;
#pragma warning disable CS8618

namespace PaymentService.Application.CQRS.Pumps.Commands.CreatePump
{
    public class CreatePumpCommand : IRequest<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public int ModelId { get; set; }

        public int FilialId { get; set; }

        public int Status { get; set; }
    }
}
