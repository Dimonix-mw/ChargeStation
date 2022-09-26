using System.Numerics;

namespace ChargeService.MessageBroker.Entities
{
    /// <summary>
    /// Запрос на начало зарядки в очередь StartChargeMQRequest
    /// в микросервис PlatformService
    /// </summary>
    public class StartChargeMQRequest
    {
        /// <summary>
        /// Идентификатор Filling
        /// </summary>
        public long FillingId { get; set; }
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// Идентификатор зарядки
        /// </summary>
        public int PumpId { get; set; }
        /// <summary>
        /// Идентификатор промоакции
        /// </summary>
        public int? PromotionId { get; set; }
        /// <summary>
        /// Сумма по промоакции
        /// </summary>
        public decimal? PromotionAmount { get; set; }
        /// <summary>
        /// Всего денег
        /// </summary>
        public decimal? TotalMoneyAmount { get; set; }
        /// <summary>
        /// Сумма бонуса
        /// </summary>
        public decimal? BonusAmount { get; set; }
        /// <summary>
        /// Идентификатор расчета бонуса
        /// </summary>
        public int? BonusCalculateRuleId { get; set; }
        /// <summary>
        /// FK Session.Id
        /// </summary>
        public long SessionId { get; set; }
        /// <summary>
        /// Идентификатор, отправляемый мобильным устройством
        /// и уникально характеризующий запрос с мобильного устройства
        /// </summary>
        public Guid RequestId { get; set; }
        /// <summary>
        /// Дата создания запроса
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Время зарядки, в запросе от мобильного устройства
        /// </summary>
        public int Minutes { get; set; }
    }
}
