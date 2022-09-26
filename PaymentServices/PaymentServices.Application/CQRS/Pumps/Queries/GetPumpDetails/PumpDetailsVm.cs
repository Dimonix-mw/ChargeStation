using AutoMapper;
using PaymentService.Domain;
using PaymentService.Application.Common.Mappings;
#pragma warning disable CS8618

namespace PaymentService.Application.CQRS.Pumps.Queries.GetPumpDetails
{
    public class PumpDetailsVm : IMapWith<Pump>
    {
        public string Name { get; set; }

        public int Number { get; set; }

        public int ModelId { get; set; }
        public string PumpModelName { get; set; }

        public int FilialId { get; set; }
        public string FilialName { get; set; }

        public int Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Pump, PumpDetailsVm>()
                .ForMember(pd => pd.FilialId,
                    opt => opt.MapFrom(p => p.FilialId))
                .ForMember(pd => pd.Number,
                    opt => opt.MapFrom(p => p.Number))
                .ForMember(pd => pd.Status,
                    opt => opt.MapFrom(p => p.Status))
                .ForMember(pd => pd.Name,
                    opt => opt.MapFrom(p => p.Name))
                .ForMember(pd => pd.ModelId,
                    opt => opt.MapFrom(p => p.ModelId))
                .ForMember(pd => pd.PumpModelName,
                    opt => opt.MapFrom(p => p.Model.Name))
                .ForMember(pd => pd.FilialName,
                    opt => opt.MapFrom(p => p.Filial.Name));
        }
    }
}