using MediatR;

namespace PaymentService.Application.CQRS.Pumps.Queries.GetPumpDetails
{
    public class GetPumpDetailsQuery : IRequest<PumpDetailsVm>
    {
        public int Id { get; set; }
    }
}
