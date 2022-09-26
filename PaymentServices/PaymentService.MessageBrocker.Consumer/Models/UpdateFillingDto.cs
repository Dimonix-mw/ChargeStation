using AutoMapper;
using PaymentService.Application.Common.Mappings;
using PaymentService.Application.CQRS.Fillings.Commands.UpdateFilling;

namespace PaymentService.MessageBrocker.Consumer.Models
{
    public class UpdateFillingDto : IMapWith<UpdateFillingCommand>
    {
        public int Id { get; set; }
        public int PumpId { get; set; }

        public int PromotionId { get; set; }

        public decimal PromotionAmount { get; set; }

        public decimal TotalMoneyAmount { get; set; }

        public decimal BonusAmount { get; set; }

        public int BonusCalculateRuleId { get; set; }

        public int Minutes { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateFillingDto, UpdateFillingCommand>();
        }
    }
}