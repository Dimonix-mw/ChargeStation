using MediatR;

namespace PaymentService.Application.CQRS.Fillings.Commands.DeleteFilling
{
    public class DeleteFillingCommand : IRequest
    {
        public int Id { get; set; }
    }
}
