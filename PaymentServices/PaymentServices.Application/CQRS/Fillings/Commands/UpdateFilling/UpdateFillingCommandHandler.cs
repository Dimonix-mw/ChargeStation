using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Fillings.Commands.UpdateFilling
{
    public class UpdateFillingCommandHandler : IRequestHandler<UpdateFillingCommand>
    {
        private readonly IPaymentServiceDbContext _dbContext;

        public UpdateFillingCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateFillingCommand request, CancellationToken cancellationToken)
        {
            var entity = 
                await _dbContext.Fillings.FirstOrDefaultAsync(x =>
                    x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Filling), request.Id);
            }

            entity.TotalMoneyAmount = request.TotalMoneyAmount;
            entity.PromotionAmount = request.PromotionAmount;
            entity.PromotionId = request.PromotionId;
            entity.Minutes = request.Minutes;
            entity.PumpId = request.PumpId;
            entity.BonusCalculateRuleId = request.BonusCalculateRuleId;
            entity.BonusAmount = request.BonusAmount;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
