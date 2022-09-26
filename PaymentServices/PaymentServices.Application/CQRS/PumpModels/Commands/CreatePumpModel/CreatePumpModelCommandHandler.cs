using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.PumpModels.Commands.CreatePumpModel
{
    public class CreatePumpModelCommandHandler : IRequestHandler<CreatePumpModelCommand, int>
    {
        private readonly IPaymentServiceDbContext _dbContext;

        public CreatePumpModelCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreatePumpModelCommand request, CancellationToken cancellationToken)
        {
            var entity = new PumpModel
            {
                Id = request.Id,
                Name = request.Name,
                is_quick_charge = request.is_quick_charge
            };

            await _dbContext.PumpModels.AddAsync(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
