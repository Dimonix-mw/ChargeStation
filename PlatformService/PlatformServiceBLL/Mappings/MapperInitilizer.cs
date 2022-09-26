using AutoMapper;
using PlatformService.MessageBroker.Common.Models;
using PlatformServiceBLL.DTOs;
using PlatformServiceDAL.Entities;

namespace PlatformServiceBLL.Mappings
{
    public class MapperInitilizer : Profile
    {
        public MapperInitilizer()
        {
            CreateMap<Filling, StartChargeMQRequest>()
                .ForMember(dest => dest.FillingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PumpId, opt => opt.MapFrom(src => src.PumpId))
                .ForMember(dest => dest.Minutes, opt => opt.MapFrom(src => src.Minutes))
                .ForMember(dest => dest.BonusCalculateRuleId, opt => opt.MapFrom(src => src.BonusCalculateRuleId))
                .ForMember(dest => dest.BonusAmount, opt => opt.MapFrom(src => src.BonusAmount))
                .ForMember(dest => dest.PromotionAmount, opt => opt.MapFrom(src => src.PromotionAmount))
                .ForMember(dest => dest.TotalMoneyAmount, opt => opt.MapFrom(src => src.TotalMoneyAmount))
                .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.PromotionId))
                .ReverseMap();

            CreateMap<Session, StartChargeMQRequest>()
                .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.RequestId))
                .ForMember(dest => dest.Minutes, opt => opt.MapFrom(src => src.Minutes))
                .ForMember(dest => dest.FillingId, opt => opt.MapFrom(src => src.FillingId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created))
                .ReverseMap();

            CreateMap<FillingDto, Filling>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PumpId, opt => opt.MapFrom(src => src.PumpId))
                .ForMember(dest => dest.Minutes, opt => opt.MapFrom(src => src.Minutes))
                .ForMember(dest => dest.BonusCalculateRuleId, opt => opt.MapFrom(src => src.BonusCalculateRuleId))
                .ForMember(dest => dest.BonusAmount, opt => opt.MapFrom(src => src.BonusAmount))
                .ForMember(dest => dest.PromotionAmount, opt => opt.MapFrom(src => src.PromotionAmount))
                .ForMember(dest => dest.TotalMoneyAmount, opt => opt.MapFrom(src => src.TotalMoneyAmount))
                .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.PromotionId))
                .ReverseMap();
        }
    }
}
