using BadBroker.Api.Models;

namespace BadBroker.Api.Controllers
{
    public interface IRatesService
    {
        BestRatesResponse GetBestRatesFor(DateTime startDate, DateTime endDate, double moneyUsd);
    }

    public class RatesService : IRatesService
    {
        public BestRatesResponse GetBestRatesFor(DateTime startDate, DateTime endDate, double moneyUsd)
        {
            throw new NotImplementedException();
        }
    }
}