namespace ChargeService.Kafka.Common.Entities;

public class InsertPumpRequest
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public long UserId { get; set; }
    /// <summary>
    /// Идентификатор, отправляемый мобильным устройством
    /// и уникально характеризующий запрос с мобильного устройства
    /// </summary>
    public Guid RequestId { get; set; }
    /// <summary>
    /// Время зарядки, в запросе от мобильного устройства
    /// </summary>
    public int Minutes { get; set; }
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
}

