using RateCheck.Services;

namespace RateCheck.Web
{
    public interface ICalculationService
    {
        CalculatedData Process(ReturnedData returnedData,decimal enteredAmount);
    }
}