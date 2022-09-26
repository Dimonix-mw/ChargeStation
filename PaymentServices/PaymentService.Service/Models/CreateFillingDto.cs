using AutoMapper;
using PaymentService.Application.Common.Mappings;
using PaymentService.Application.CQRS.Fillings.Commands.CreateFilling;

namespace PaymentService.MessageBrocker.Consumer.Models
{
    public class CreateFillingDto : IMapWith<CreateFillingCommand>
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
            profile.CreateMap<CreateFillingDto, CreateFillingCommand>();
        }
    }
}
