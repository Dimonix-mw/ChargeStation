using MediatR;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Domain;
using PaymentService.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PaymentService.Application.CQRS.Prices.Queries.GetPriceDetails;
using Microsoft.EntityFrameworkCore;

namespace PaymentService.Application.CQRS.Prices.Queries.GetPriceList
{
    public class GetPriceListQueryHandler : IRequestHandler<GetPriceListQuery, PriceList>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentServiceDbContext _dbContext;

        public GetPriceListQueryHandler(IPaymentServiceDbContext dbContext, IMapper mapper)
            => (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<PriceList> Handle(GetPriceListQuery request, CancellationToken cancellationToken)
        {
            dynamic entitys;
            if (request.FilialId == 0 && request.PumpModelId == 0) {
                throw new NotFoundException(nameof(Price), request.PumpModelId);
            } else if (request.FilialId == 0 && request.PumpModelId != 0)
            {
                entitys = await _dbContext.Prices
                    .Where(x => x.PumpModelId == request.PumpModelId)
                    .ProjectTo<PriceDetails>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            } else if (request.FilialId != 0 && request.PumpModelId == 0)
            {
                entitys = await _dbContext.Prices
                   .Where(x => x.FilialId == request.FilialId)
                   .ProjectTo<PriceDetails>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
            } else
            {
                entitys = await _dbContext.Prices
                   .Where(x => x.FilialId == request.FilialId && x.PumpModelId == request.PumpModelId)
                   .ProjectTo<PriceDetails>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
            }

            return new PriceList { Prices = entitys };
        }
    }
}
