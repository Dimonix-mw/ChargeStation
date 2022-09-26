using MediatR;

namespace PaymentService.Application.CQRS.Prices.Commands.UpdatePrice
{
    public class UpdatePriceCommand : IRequest
    {
        public int Id { get; set; }

        public int FilialId { get; set; }

        public int PumpModelId { get; set; }

        public decimal Cost { get; set; }
    }
}
