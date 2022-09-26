namespace HardwareEmulator.Models
{
    /// <summary>
    /// Запрос на начало зарядки
    /// </summary>
    public class StartHardwareCharge
    {
        /// <summary>
        /// Идентификатор, отправляемый мобильным устройством
        /// и уникально характеризующий запрос с мобильного устройства
        /// </summary>
        public Guid RequestId { get; set; }
        /// <summary>
        /// Время зарядки, в запросе от мобильного устройства
        /// </summary>
        public int Minutes { get; set; }
    }
}
