using PaymentService.Application.CQRS.Prices.Queries.GetPriceDetails;
#pragma warning disable CS8618

namespace PaymentService.Application.CQRS.Prices.Queries.GetPriceList
{
    public class PriceList
    {
        public IList<PriceDetails> Prices { get; set; }
    }
}
