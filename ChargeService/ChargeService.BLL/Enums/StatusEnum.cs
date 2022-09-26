namespace ChargeService.BLL.Enums
{
    /// <summary>
    /// Статус зарядки
    /// </summary>
    public enum StatusEnum
    {
        /// <summary>
        /// Зарядка еще не началась
        /// </summary>
        NotStarted = 0,
        /// <summary>
        /// Идет процесс зарядки
        /// </summary>
        Charged = 1,
        /// <summary>
        /// Зарядка завершена
        /// </summary>
        End = 2,
        /// <summary>
        /// Статус зарядки неизвестен
        /// </summary>
        Unknown = 3,
        /// <summary>
        /// Зарядка принудительно завершена
        /// </summary>
        Rejected = 4
    }
}
