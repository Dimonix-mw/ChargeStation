using AutoMapper;
using PaymentService.Application.Common.Mappings;
using PaymentService.Application.CQRS.Pumps.Queries.GetPumpDetails;

namespace PaymentService.MessageBrocker.Consumer.Models
{
    public class GetPumpDetailsDto : IMapWith<GetPumpDetailsQuery>
    {
        public int Id { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetPumpDetailsDto, GetPumpDetailsQuery>();
        }
    }
}