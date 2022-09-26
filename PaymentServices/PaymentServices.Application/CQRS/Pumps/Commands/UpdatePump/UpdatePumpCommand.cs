using MediatR;
#pragma warning disable CS8618

namespace PaymentService.Application.CQRS.Pumps.Commands.UpdatePump
{
    public class UpdatePumpCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public int ModelId { get; set; }

        public int FilialId { get; set; }

        public int Status { get; set; }
    }
}
