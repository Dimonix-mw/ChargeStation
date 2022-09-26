using AutoMapper;
using ChargeService.BLL.Dtos;
using ChargeService.DAL.Entities;
using ChargeService.MessageBroker.Entities;

namespace ChargeService.BLL.Mappings
{
    public class MapperInitilizer : Profile
    {
        public MapperInitilizer()
        {
            CreateMap<Session, InsertPumpRequestDto>()
                .ForMember(dest => dest.PumpId, opt => opt.MapFrom(src => src.Filling.PumpId))
                .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.Filling.PromotionId))
                .ForMember(dest => dest.TotalMoneyAmount, opt => opt.MapFrom(src => src.Filling.TotalMoneyAmount))
                .ForMember(dest => dest.BonusAmount, opt => opt.MapFrom(src => src.Filling.BonusAmount))
                .ForMember(dest => dest.BonusCalculateRuleId, opt => opt.MapFrom(src => src.Filling.BonusCalculateRuleId))
                .ForMember(dest => dest.PromotionAmount, opt => opt.MapFrom(src => src.Filling.PromotionAmount))
                .ReverseMap();
            CreateMap<Session, CheckPumpStatusResponseDto>().ReverseMap();
            CreateMap<Session, StartChargeMQRequest>()
                .ForMember(dest => dest.PumpId, opt => opt.MapFrom(src => src.Filling.PumpId))
                .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.Filling.PromotionId))
                .ForMember(dest => dest.TotalMoneyAmount, opt => opt.MapFrom(src => src.Filling.TotalMoneyAmount))
                .ForMember(dest => dest.BonusAmount, opt => opt.MapFrom(src => src.Filling.BonusAmount))
                .ForMember(dest => dest.BonusCalculateRuleId, opt => opt.MapFrom(src => src.Filling.BonusCalculateRuleId))
                .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FillingId, opt => opt.MapFrom(src => src.FillingId))
                .ReverseMap();
        }
    }
}
