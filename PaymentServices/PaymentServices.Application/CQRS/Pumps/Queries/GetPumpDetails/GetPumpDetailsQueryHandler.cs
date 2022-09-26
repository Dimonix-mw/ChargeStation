using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Pumps.Queries.GetPumpDetails
{
    public class GetPumpDetailsQueryHandler : IRequestHandler<GetPumpDetailsQuery, PumpDetailsVm>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentServiceDbContext _dbContext;

        public GetPumpDetailsQueryHandler(IPaymentServiceDbContext dbContext, IMapper mapper)
            => (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<PumpDetailsVm> Handle(GetPumpDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Pumps.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Pump), request.Id);
            }

            return _mapper.Map<PumpDetailsVm>(entity);
        }
    }
}
