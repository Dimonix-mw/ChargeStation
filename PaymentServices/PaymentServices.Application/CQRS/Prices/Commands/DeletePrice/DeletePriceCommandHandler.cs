using MediatR;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Prices.Commands.DeletePrice
{
    public class DeletePriceCommandHandler : IRequestHandler<DeletePriceCommand>
    {
        private readonly IPaymentServiceDbContext _dbContext;

        public DeletePriceCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeletePriceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Prices.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Price), request.Id);
            }

            _dbContext.Prices.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
