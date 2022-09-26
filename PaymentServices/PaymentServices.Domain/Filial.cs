#pragma warning disable CS8618
namespace PaymentService.Domain
{
    public class Filial
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
        public List<Price>? Prices { get; set; }
        public List<Pump>? Pumps { get; set; }

        /*public Filial(string name)
        {
            Name = name;
        }*/
    }
}
