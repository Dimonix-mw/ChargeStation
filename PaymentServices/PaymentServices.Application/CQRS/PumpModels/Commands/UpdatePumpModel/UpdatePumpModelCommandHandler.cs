using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.PumpModels.Commands.UpdatePumpModel
{
    public class UpdatePumpModelCommandHandler : IRequestHandler<UpdatePumpModelCommand>
    {
        private readonly IPaymentServiceDbContext _dbContext;

        public UpdatePumpModelCommandHandler(IPaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdatePumpModelCommand request, CancellationToken cancellationToken)
        {
            var entity = 
                await _dbContext.PumpModels.FirstOrDefaultAsync(x => 
                    x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(PumpModel), request.Id);
            }

            entity.Name = request.Name;
            entity.is_quick_charge = request.is_quick_charge;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
