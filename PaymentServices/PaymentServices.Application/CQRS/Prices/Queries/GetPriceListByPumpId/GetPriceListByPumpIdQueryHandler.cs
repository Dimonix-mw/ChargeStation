using MediatR;
using PaymentService.Application.Common.Exceptions;
using PaymentService.Domain;
using PaymentService.Application.CQRS.Prices.Queries.GetPriceDetails;
using PaymentService.Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PaymentService.Application.CQRS.Prices.Queries.GetPriceList;

namespace PaymentService.Application.CQRS.Prices.Queries.GetPriceListByPumpId
{
    public class GetPriceListByPumpIdQueryHandler : IRequestHandler<GetPriceListByPumpIdQuery, PriceList>
    {
        private readonly IPaymentServiceDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetPriceListByPumpIdQueryHandler(IPaymentServiceDbContext dbContext, IMapper mapper)
            => (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<PriceList> Handle(GetPriceListByPumpIdQuery request, CancellationToken cancellationToken)
        {
            var query = from pump in _dbContext.Pumps
                                      join price in _dbContext.Prices on
                                         new { FilialId = pump.FilialId, ModelId = pump.ModelId }
                                             equals
                                         new { FilialId = price.FilialId, ModelId = price.PumpModelId }
                                      join filial in _dbContext.Filials on pump.FilialId equals filial.Id
                                      join pumpModel in _dbContext.PumpModels on pump.ModelId equals pumpModel.Id
                                      where pump.Id == request.PumpId
                                      select price;
                         
            
            var prices = await query.ProjectTo<PriceDetails>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
            if (prices.Count == 0) {
                throw new NotFoundException($"Not found need data from Prices by PumpId = {request.PumpId}");
            }
            return new PriceList { Prices = prices };
        }
    }
}
