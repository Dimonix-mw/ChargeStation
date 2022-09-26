#pragma warning disable CS8618
namespace PaymentService.Domain
{
    public class Price
    {
        /// <summary>
        /// Идентификатор Price
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// FK Filial.Id
        /// </summary>
        public int FilialId { get; set; }
        public Filial Filial { get; set; }

        /// <summary>
        /// FK PumpModel.Id
        /// </summary>
        public int PumpModelId { get; set; }
        public PumpModel PumpModel { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal Cost { get; set; }
    }
}
