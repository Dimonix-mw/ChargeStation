namespace PaymentService.Application.Common.Exceptions
{
    public class DataAlreadyExistsException : Exception
    {
        public DataAlreadyExistsException(string name, object key)
            : base($"Entity \"{name}\" ({key}) already exists.") { }
    }
}
