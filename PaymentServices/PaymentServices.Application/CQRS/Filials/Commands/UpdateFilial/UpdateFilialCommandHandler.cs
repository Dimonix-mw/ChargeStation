using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Filials.Commands.UpdateFilial
{
    public class UpdateFilialCommandHandler : IRequestHandler<UpdateFilialCommand>
    {
        private readonly IPaymentServiceDbContext _dbContext;

        public UpdateFilialCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateFilialCommand request, CancellationToken cancellationToken)
        {
            var entity = 
                await _dbContext.Filials.FirstOrDefaultAsync(x =>
                    x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Filial), request.Id);
            }

            entity.Name = request.Name;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
