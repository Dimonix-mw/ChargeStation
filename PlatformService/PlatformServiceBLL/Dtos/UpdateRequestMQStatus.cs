namespace PlatformServiceBLL.DTOs
{
    /// <summary>
    /// Обновление статуса зарядки - 
    /// возвращается от микросервиса PlatformService в очередь UpdateRequestMQStatus
    /// </summary>
    public class UpdateRequestMQStatus
    {
        /// <summary>
        /// Идентификатор, отправляемый мобильным устройством
        /// и уникально характеризующий запрос с мобильного устройства
        /// </summary>
        public Guid RequestId { get; set; }
        /// <summary>
        /// Статус зарядки
        /// 0 - Зарядка еще не началась
        /// 1 - Идет процесс зарядки
        /// 2 - Зарядка завершена
        /// 3 - Статус зарядки неизвестен
        /// 4 - Зарядка принудительно завершена
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public long UserId { get; set; }
        
        public decimal? TotalMoneyAmount { get; set; }
        public int Minutes { get; set; }
        /// <summary>
        /// Идентификатор зарядки
        /// </summary>
        public int PumpId { get; set; }
    }
}
