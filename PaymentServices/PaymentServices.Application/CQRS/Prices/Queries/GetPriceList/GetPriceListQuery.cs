using MediatR;

namespace PaymentService.Application.CQRS.Prices.Queries.GetPriceList
{
    public class GetPriceListQuery : IRequest<PriceList>
    {
        public int PumpModelId { get; set; }
        public int FilialId { get; set; }
    }
}
