using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformServiceBLL.DTOs
{
    public class FillingDto
    {
        public long Id { get; set; }

        public int PumpId { get; set; }

        public int PromotionId { get; set; }

        public decimal PromotionAmount { get; set; }

        public decimal TotalMoneyAmount { get; set; }

        public decimal BonusAmount { get; set; }

        public int BonusCalculateRuleId { get; set; }

        public int Minutes { get; set; }

    }

}
