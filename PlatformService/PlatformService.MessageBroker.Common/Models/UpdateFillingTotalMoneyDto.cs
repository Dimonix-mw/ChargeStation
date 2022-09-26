namespace PlatformService.MessageBroker.Common.Models
{
    //ответ от PaymetService о стоимости заправки
    public class UpdateFillingTotalMoneyDto
    {
        public int Id { get; set; }
        public decimal TotalMoneyAmount { get; set; }
    }
}
