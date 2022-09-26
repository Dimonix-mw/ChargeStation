using MediatR;

namespace PaymentService.Application.CQRS.Prices.Queries.GetPriceDetails
{
    public class GetPriceDetailsQuery : IRequest<PriceDetails>
    {
        public int Id { get; set; }
    }
}
