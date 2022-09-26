using AutoMapper;
using PaymentService.Application.Common.Mappings;
using PaymentService.Application.CQRS.Prices.Queries.GetPriceList;

namespace PaymentService.MessageBrocker.Consumer.Models
{
    public class GetPriceListDto : IMapWith<GetPriceListQuery>
    {
        public int PumpModelId { get; set; }
        public int FilialId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetPriceListDto, GetPriceListQuery>();
        }
    }
}
