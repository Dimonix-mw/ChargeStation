using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Pumps.Commands.UpdatePump
{
    public class UpdatePumpCommandHandler : IRequestHandler<UpdatePumpCommand>
    {
        private readonly IPaymentServiceDbContext _dbContext;

        public UpdatePumpCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdatePumpCommand request, CancellationToken cancellationToken)
        {
            var entity = 
                await _dbContext.Pumps.FirstOrDefaultAsync(x => 
                    x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Pump), request.Id);
            }

            entity.Number = request.Number;
            entity.Status = request.Status;
            entity.Name = request.Name;
            entity.FilialId = request.FilialId;
            entity.ModelId = request.ModelId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
