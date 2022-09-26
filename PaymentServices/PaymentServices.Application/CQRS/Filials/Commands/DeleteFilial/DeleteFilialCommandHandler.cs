using MediatR;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Filials.Commands.DeleteFilial
{
    public class DeleteFilialCommandHandler : IRequestHandler<DeleteFilialCommand>
    {
        private readonly IPaymentServiceDbContext _dbContext;

        public DeleteFilialCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteFilialCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Filials.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Filial), request.Id);
            }

            _dbContext.Filials.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}
