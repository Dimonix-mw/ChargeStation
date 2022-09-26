namespace ChargeService.BLL.Dtos
{
    /// <summary>
    /// Ответ о статусе зарядки
    /// </summary>
    public class CheckPumpStatusResponseDto
    {
        /// <summary>
        /// Статус зарядки
        /// 0 - Зарядка еще не началась
        /// 1 - Идет процесс зарядки
        /// 2 - Зарядка завершена
        /// 3 - Статус зарядки неизвестен
        /// 4 - Зарядка принудительно завершена
        /// </summary>
        public int Status { get; set; }
    }
}
