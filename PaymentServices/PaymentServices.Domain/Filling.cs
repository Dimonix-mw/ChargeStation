namespace PaymentService.Domain
{
    /// <summary>
    /// Подача
    /// </summary>
    public class Filling
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор зарядки
        /// </summary>
        public int PumpId { get; set; }

        /// <summary>
        /// Идентификатор промоакции
        /// </summary>
        public int PromotionId { get; set; }

        /// <summary>
        /// Сумма по промоакции
        /// </summary>
        public decimal PromotionAmount { get; set; }

        /// <summary>
        /// Всего денег
        /// </summary>
        public decimal TotalMoneyAmount { get; set; }

        /// <summary>
        /// Сумма бонуса
        /// </summary>
        public decimal BonusAmount { get; set; }

        /// <summary>
        /// Идентификатор расчета бонуса
        /// </summary>
        public int BonusCalculateRuleId { get; set; }

        /// <summary>
        /// FK Session.Id
        /// </summary>
        //public int SessionId { get; set; }

        /// <summary>
        /// FK Entity
        /// </summary>
        //public Session Session { get; set; }

        /// <summary>
        /// Время зарядки, в запросе от мобильного устройства
        /// </summary>
        public int Minutes { get; set; }
    }
}
