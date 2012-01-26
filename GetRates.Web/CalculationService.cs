using GetRates.Services;

namespace GetRates.Web
{
    public class CalculationService : ICalculationService
    {
        public CalculatedData Process(ReturnedData returnedData, decimal enteredAmount)
        {
            var finalAmount = (returnedData.Rate*enteredAmount) - returnedData.Deductions;
            return new CalculatedData(returnedData)
                       {
                           Amount = finalAmount,
                           EffectiveRate = finalAmount / (enteredAmount + returnedData.Fee),
                           AmountWithdrawn = enteredAmount+returnedData.Fee
                       };
        }
    }
}