#pragma warning disable CS8618
namespace PaymentService.Domain
{
    public class PumpModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True - быстрая зарядка
        /// </summary>
        public bool is_quick_charge { get; set; }

        public List<Price>? Prices { get; set; }
        public List<Pump>? Pumps { get; set; }

        /*public PumpModel(string name)
        {
            Name = name;
        }*/
    }
}
