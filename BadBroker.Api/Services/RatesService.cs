using BadBroker.Api.Models;
using BadBroker.Api.Repositories;

namespace BadBroker.Api.Services
{
    public interface IRatesService
    {
        BestRatesResponse GetBestRatesFor(DateTime startDate, DateTime endDate, double moneyUsd);
    }

    public class RatesService : IRatesService
    {
        private IRatesRepository _ratesRepository;

        public RatesService(IRatesRepository ratesRepository)
        {
            this._ratesRepository = ratesRepository;
        }

        public BestRatesResponse GetBestRatesFor(DateTime startDate, DateTime endDate, double moneyUsd)
        {
            List<Currency> currencies = Enum.GetValues(typeof(Currency)).Cast<Currency>().ToList();
            List<ExchangeRate> rates = _ratesRepository.GetExchangeRatesFor(startDate, endDate);
            BestRatesResponse bestRate = new BestRatesResponse();
            bestRate.rates = mapToRatesDto(rates);

            foreach( Currency currency in currencies) 
            {
                List<ExchangeRate> ratesForSpecificCurrency = rates.FindAll(r => r.currency == currency);
                ratesForSpecificCurrency.OrderBy(r => r.date);

                foreach(ExchangeRate buyDateRate in ratesForSpecificCurrency)
                {
                    foreach(ExchangeRate sellDateRate in ratesForSpecificCurrency)
                    {
                        if(sellDateRate.date > buyDateRate.date) 
                        {
                            double revenue = determineRevenueFor(buyDateRate, sellDateRate, moneyUsd);
                            if(revenue > bestRate.revenue)
                            {
                                bestRate.buyDate = buyDateRate.date;
                                bestRate.sellDate = sellDateRate.date;
                                bestRate.tool = currency;
                                bestRate.revenue = revenue;
                            }
                        }
                    }
                }

            }
            return bestRate;
        }

        private IEnumerable<RateDto> mapToRatesDto(List<ExchangeRate> rates)
        {
            return rates.GroupBy(r => r.date).Select(g => 
                new RateDto()
                {
                    date = g.Key,
                    rub = g.Any( e => e.currency == Currency.RUB) ? g.First( e => e.currency == Currency.RUB).value : null,
                    eur = g.Any( e => e.currency == Currency.EUR) ? g.First( e => e.currency == Currency.EUR).value : null,
                    gbp = g.Any( e => e.currency == Currency.GBP) ? g.First( e => e.currency == Currency.GBP).value : null,
                    jpy = g.Any( e => e.currency == Currency.JPY) ? g.First( e => e.currency == Currency.JPY).value : null
                }
            );
        }

        private double determineRevenueFor(ExchangeRate buyDateRate, ExchangeRate sellDateRate, double moneyUsd)
        {
            double initialValue = buyDateRate.value * moneyUsd / sellDateRate.value;
            double fees = (sellDateRate.date - buyDateRate.date).Days * 1;
            return initialValue - fees - moneyUsd;
        }
    }
}