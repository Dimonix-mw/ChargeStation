using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Prices.Commands.CreatePrice
{
    public class CreatePriceCommandHandler : IRequestHandler<CreatePriceCommand, int>
    {
        private readonly IPaymentServiceDbContext _dbContext;
        public CreatePriceCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreatePriceCommand request, CancellationToken cancellationToken)
        {
            var entity = new Price
            {
                //Id = request.Id,
                FilialId = request.FilialId,
                PumpModelId = request.PumpModelId,
                Cost = request.Cost
            };

            await _dbContext.Prices.AddAsync(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
