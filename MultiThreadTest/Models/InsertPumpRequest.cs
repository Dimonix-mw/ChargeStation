using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreadTest.Models
{
    /// <summary>
    /// Запрос на зарядку от мобильного устройства
    /// </summary>
    public class InsertPumpRequest
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public int UserId { get; set; }
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

        public override string ToString()
        {
            return $"RequestId = {RequestId}";
        }
    }
}
