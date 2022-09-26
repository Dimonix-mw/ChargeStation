namespace PaymentService.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) not found.") { }

        public NotFoundException(string message)
            : base(message) { }
    }
}
