using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeService.BLL.Dtos
{
    /// <summary>
    /// Запрос на проверку статуса
    /// зарядной станции
    /// </summary>
    public class CheckPumpStatusRequestDto
    {
        /// <summary>
        /// Идентификатор, отправляемый мобильным устройством
        /// и уникально характеризующий запрос с мобильного устройства
        /// </summary>
        public Guid RequestId { get; set; }
    }
}
