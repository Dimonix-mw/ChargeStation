using MediatR;

namespace PaymentService.Application.CQRS.Prices.Commands.DeletePrice
{
    public class DeletePriceCommand : IRequest
    {
        public int Id { get; set; }
    }
}
