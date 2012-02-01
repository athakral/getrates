using GetRates.Services;

namespace GetRates.Web
{
    public class CalculationService : ICalculationService
    {
        public CalculatedData Process(ReturnedData returnedData, decimal enteredAmount)
        {
            CalculatedData data = null;
            if (returnedData != null)
            {
                var finalAmount = (returnedData.Rate * enteredAmount) - returnedData.Deductions;
                data= new CalculatedData(returnedData)
                           {
                               Amount = finalAmount,
                               EffectiveRate = finalAmount / (enteredAmount + returnedData.Fee),
                               AmountWithdrawn = enteredAmount + returnedData.Fee
                           };
            }
            return data;
        }
    }
}