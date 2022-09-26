using AutoMapper;
using PaymentService.Application.Common.Mappings;
using PaymentService.Application.CQRS.Prices.Queries.GetPriceListByPumpId;

namespace PaymentService.MessageBrocker.Consumer.Models
{
    public class GetPriceListByPumpIdDto : IMapWith<GetPriceListByPumpIdQuery>
    {
        public int PumpId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetPriceListByPumpIdDto, GetPriceListByPumpIdQuery>();
        }
    }
}
