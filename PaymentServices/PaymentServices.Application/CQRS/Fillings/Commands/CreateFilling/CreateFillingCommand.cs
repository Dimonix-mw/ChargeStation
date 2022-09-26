using MediatR;

namespace PaymentService.Application.CQRS.Fillings.Commands.CreateFilling
{
    public class CreateFillingCommand : IRequest<int>
    {
        public int Id { get; set; }

        public int PumpId { get; set; }

        public int PromotionId { get; set; }

        public decimal PromotionAmount { get; set; }

        public decimal TotalMoneyAmount { get; set; }

        public decimal BonusAmount { get; set; }

        public int BonusCalculateRuleId { get; set; }

        public int Minutes { get; set; }

    }
}
