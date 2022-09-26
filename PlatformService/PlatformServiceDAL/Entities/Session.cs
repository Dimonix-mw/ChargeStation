using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformServiceDAL.Entities {
    /// <summary>
    /// Представляет сессию зарядки
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }
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
        /// Дата создания запроса
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Время зарядки, в запросе от мобильного устройства
        /// </summary>
        public int Minutes { get; set; }
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
        /// Filling
        /// </summary>
        public Filling Filling { get; set; }
        /// <summary>
        /// FK Filling
        /// </summary>
        public long FillingId { get; set; }
    }

}
