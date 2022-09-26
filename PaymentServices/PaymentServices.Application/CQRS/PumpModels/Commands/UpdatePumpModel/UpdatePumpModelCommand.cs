using MediatR;
#pragma warning disable CS8618

namespace PaymentService.Application.CQRS.PumpModels.Commands.UpdatePumpModel
{
    public class UpdatePumpModelCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool is_quick_charge { get; set; }
    }
}
