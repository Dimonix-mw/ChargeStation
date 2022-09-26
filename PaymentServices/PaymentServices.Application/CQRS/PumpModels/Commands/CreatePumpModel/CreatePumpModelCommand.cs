using MediatR;
#pragma warning disable CS8618

namespace PaymentService.Application.CQRS.PumpModels.Commands.CreatePumpModel
{
    public class CreatePumpModelCommand : IRequest<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool is_quick_charge { get; set; }
    }
}
