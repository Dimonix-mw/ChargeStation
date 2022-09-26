#pragma warning disable CS8618
namespace PaymentService.Domain
{
    public class Pump
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
        /// Номер заправки
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// FK PumpModel.Id
        /// </summary>
        public int ModelId { get; set; }
        public PumpModel Model { get; set; }

        /// <summary>
        /// FK Filial.Id
        /// </summary>
        public int FilialId { get; set; }
        public Filial Filial { get; set; }

        /// <summary>
        /// Состояние заправки
        /// 0 - Свободна
        /// 1 - Идет процесс зарядки
        /// 2 - Не доступна
        /// 3 - Отключена
        /// </summary>
        public int Status { get; set; }

        /*public Pump(string name)
        {
            Name = name;
        }*/
    }
}
