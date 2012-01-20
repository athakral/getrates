namespace RateCheck.Services
{
    public interface IProviderService
    {
       float GetRate(float amount);
    }

    public class XoomProviderService : IProviderService
    {
        public float GetRate(float amount)
        {
            return (float) 0.9;
        }
    }
}