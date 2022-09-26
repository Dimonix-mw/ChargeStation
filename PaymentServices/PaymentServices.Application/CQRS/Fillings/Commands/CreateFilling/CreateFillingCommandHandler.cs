using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Fillings.Commands.CreateFilling
{
    public class CreateFillingCommandHandler : IRequestHandler<CreateFillingCommand, int>
    {
        protected readonly IPaymentServiceDbContext _dbContext;

        public CreateFillingCommandHandler(IPaymentServiceDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<int> Handle(CreateFillingCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Fillings.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity != null)
            {
                throw new DataAlreadyExistsException(nameof(Filling), request.Id);
            }
            
            entity = new Filling
            {
                Id = request.Id,
                PumpId = request.PumpId,
                Minutes = request.Minutes,
                BonusAmount = request.BonusAmount,
                BonusCalculateRuleId = request.BonusCalculateRuleId,
                PromotionAmount= request.PromotionAmount,
                PromotionId= request.PromotionId,
                TotalMoneyAmount= request.TotalMoneyAmount
            };

            await _dbContext.Fillings.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
