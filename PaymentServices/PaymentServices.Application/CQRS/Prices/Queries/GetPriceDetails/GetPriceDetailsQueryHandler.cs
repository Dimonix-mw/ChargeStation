using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Application.Interfaces;
using PaymentService.Domain;

namespace PaymentService.Application.CQRS.Prices.Queries.GetPriceDetails
{
    public class GetPriceDetailsQueryHandler : IRequestHandler<GetPriceDetailsQuery, PriceDetails>
    {
        private readonly IPaymentServiceDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetPriceDetailsQueryHandler(IPaymentServiceDbContext dbContext, IMapper mapper)
            => (_dbContext, _mapper) = (dbContext, mapper);
        
        public async Task<PriceDetails> Handle(GetPriceDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Prices.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Price), request.Id);
            }

            return _mapper.Map<PriceDetails>(entity);
        }
    }
}
