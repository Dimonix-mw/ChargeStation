using MediatR;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Pumps.Commands.DeletePump
{
    public class DeletePumpCommandHandler : IRequestHandler<DeletePumpCommand>
    {
        private readonly IPaymentServiceDbContext _dbContext;

        public DeletePumpCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeletePumpCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Pumps.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Pump), request.Id);
            }

            _dbContext.Pumps.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
