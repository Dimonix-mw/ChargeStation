using AutoMapper;
using PaymentService.Application.Common.Mappings;
using PaymentService.Domain;
#pragma warning disable CS8618

namespace PaymentService.Application.CQRS.Prices.Queries.GetPriceDetails
{
    public class PriceDetails : IMapWith<Price>
    {
        public int FilialId { get; set; }

        public int PumpModelId { get; set; }

        public decimal Cost { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Price, PriceDetails>()
                .ForMember(pd => pd.FilialId,
                    opt => opt.MapFrom(pr => pr.FilialId))
                .ForMember(pd => pd.PumpModelId,
                    opt => opt.MapFrom(pr => pr.PumpModelId))
                .ForMember(pd => pd.Cost,
                    opt => opt.MapFrom(pr => pr.Cost));

            /*.ForMember(pd => pd.FilialName,
                    opt => opt.MapFrom(pr => pr.Filial.Name))
                .ForMember(pd => pd.PumpModelName,
                    opt => opt.MapFrom(pr => pr.PumpModel.Name))*/
        }
    }
}
