using MediatR;
using PaymentService.Application.CQRS.Prices.Queries.GetPriceList;

namespace PaymentService.Application.CQRS.Prices.Queries.GetPriceListByPumpId
{
    public class GetPriceListByPumpIdQuery : IRequest<PriceList>
    {
        public int PumpId { get; set; }
    }
}
