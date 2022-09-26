using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Prices.Commands.UpdatePrice
{
    public class UpdatePriceCommandHandler : IRequestHandler<UpdatePriceCommand>
    {
        private readonly IPaymentServiceDbContext _dbContext;
        public UpdatePriceCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
        {
            var entity =
                await _dbContext.Prices.FirstOrDefaultAsync(x =>
                    x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Price), request.Id);
            }

            entity.Cost = request.Cost;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
