using Bogus;
using MultiThreadTest.Models;

namespace MultiThreadTest.Factory
{
    public class FakeFactory
    {
        public static List<InsertPumpRequest> CreateList(int numberOfEntities)
        {
            var dummyRequest = new Faker<InsertPumpRequest>()
                .RuleFor(i => i.UserId, f => f.Random.Int(0, 10000))
                .RuleFor(i => i.RequestId, f => f.Random.Uuid())
                .RuleFor(i => i.Minutes, f => f.Random.Int(10, 30))
                .RuleFor(i => i.PumpId, f => f.Random.Int(1, 10))
                .RuleFor(i => i.PromotionId, f => f.Random.Int(0, 5))
                .RuleFor(i => i.PromotionAmount, f => f.Random.Int(0, 5))
                .RuleFor(i => i.TotalMoneyAmount, f => f.Random.Int(50, 6000))
                .RuleFor(i => i.BonusAmount, f => f.Random.Int(0, 600))
                .RuleFor(i => i.BonusCalculateRuleId, f => f.Random.Int(1, 10))
                .Generate(numberOfEntities);
            return dummyRequest.ToList();
        }
    }
}
