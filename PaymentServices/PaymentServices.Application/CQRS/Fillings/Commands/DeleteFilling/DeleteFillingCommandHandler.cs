using MediatR;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Fillings.Commands.DeleteFilling
{
    public class DeleteFillingCommandHandler : IRequestHandler<DeleteFillingCommand>
    {
        private readonly IPaymentServiceDbContext _dbContext;

        public DeleteFillingCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteFillingCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Fillings.FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Filling), request.Id);
            }

            _dbContext.Fillings.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}
