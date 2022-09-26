using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Pumps.Commands.CreatePump
{
    public class CreatePumpCommandHandler : IRequestHandler<CreatePumpCommand, int>
    {
        private readonly IPaymentServiceDbContext _dbContext;

        public CreatePumpCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreatePumpCommand request, CancellationToken cancellationToken)
        {
            var entity = new Pump
            {
                Id = request.Id,
                FilialId = request.FilialId,
                ModelId= request.ModelId,
                Name = request.Name,
                Number = request.Number,
                Status = request.Status
            };

            await _dbContext.Pumps.AddAsync(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
