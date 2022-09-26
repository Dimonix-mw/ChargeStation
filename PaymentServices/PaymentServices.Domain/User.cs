#pragma warning disable CS8618
namespace PaymentService.Domain
{
    /// <summary>
    /// Заказчик, пользующийся услугами зарядки
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        public List<Session>? Sessions { get; set; }
        public List<Filling>? Filings { get; set; }

        /*public User(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }*/
    }
}
