using GetRates.Services;

namespace GetRates.Web
{
    public interface ICalculationService
    {
        CalculatedData Process(ReturnedData returnedData,decimal enteredAmount);
    }
}