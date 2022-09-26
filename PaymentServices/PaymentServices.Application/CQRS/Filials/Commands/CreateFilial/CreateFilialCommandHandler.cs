using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Filials.Commands.CreateFilial
{
    public class CreateFilialCommandHandler : IRequestHandler<CreateFilialCommand, int>
    {
        protected readonly IPaymentServiceDbContext _dbContext;

        public CreateFilialCommandHandler(IPaymentServiceDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<int> Handle(CreateFilialCommand request, CancellationToken cancellationToken)
        {
            var entity = new Filial
            {
                Id = request.Id,
                Name = request.Name
            };

            await _dbContext.Filials.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
