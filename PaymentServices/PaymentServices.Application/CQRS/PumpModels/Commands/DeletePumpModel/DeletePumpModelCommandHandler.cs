using MediatR;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.PumpModels.Commands.DeletePumpModel
{
    public class DeletePumpModelCommandHandler : IRequestHandler<DeletePumpModelCommand>
    {
        private readonly IPaymentServiceDbContext _dbContext;

        public DeletePumpModelCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeletePumpModelCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.PumpModels.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(PumpModel), request.Id);
            }

            _dbContext.PumpModels.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
