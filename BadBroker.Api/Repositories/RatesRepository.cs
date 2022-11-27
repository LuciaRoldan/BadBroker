using BadBroker.Api.Models;

namespace BadBroker.Api.Repositories
{
    public interface IRatesRepository
    {
        List<ExchangeRate>  GetExchangeRatesFor(DateTime startDate, DateTime endDate);
    }

    public class RatesRepository : IRatesRepository
    {
        public List<ExchangeRate>  GetExchangeRatesFor(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}