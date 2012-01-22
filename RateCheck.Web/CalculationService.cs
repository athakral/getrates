using RateCheck.Services;

namespace RateCheck.Web
{
    public class CalculationService : ICalculationService
    {
        public CalculatedData Process(ReturnedData returnedData, decimal enteredAmount)
        {
            var finalAmount = (returnedData.Rate*enteredAmount) - returnedData.Deductions;
            return new CalculatedData(returnedData)
                       {
                           Amount = finalAmount,
                           EffectiveRate = finalAmount/enteredAmount,
                           AmountWithdrawn = enteredAmount+returnedData.Fee
                       };
        }
    }
}