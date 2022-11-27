using BadBroker.Api.Models;
using BadBroker.Api.Clients;
using BadBroker.Api.Exceptions;

namespace BadBroker.Api.Services
{
    public interface IRatesService
    {
        Task<BestRatesResponse> GetBestRatesFor(DateTime startDate, DateTime endDate, double moneyUsd);
    }

    public class RatesService : IRatesService
    {
        private IExchangeRatesClient _ratesClient;
        private double _brokerFee;

        public RatesService(IExchangeRatesClient ratesClient, IConfiguration config)
        {
            this._ratesClient = ratesClient;
            this._brokerFee = Convert.ToDouble(config["BrokerFee"]);
        }

        public async Task<BestRatesResponse> GetBestRatesFor(DateTime startDate, DateTime endDate, double moneyUsd)
        {
            List<Currency> currencies = Enum.GetValues(typeof(Currency)).Cast<Currency>().ToList();
            List<ExchangeRate> rates = await _ratesClient.GetExchangeRatesFor(startDate, endDate);
            BestRatesResponse bestRate = new BestRatesResponse();
            bestRate.rates = MapToRatesDto(rates);

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
                            double revenue = DetermineRevenueFor(buyDateRate, sellDateRate, moneyUsd);
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
            ValidateRevenue(bestRate);
            return bestRate;
        }

        private void ValidateRevenue(BestRatesResponse bestRate)
        {
            if(bestRate.revenue <= 0)
            {
                throw new NoRevenueException();
            }
        }

        private IEnumerable<RateDto> MapToRatesDto(List<ExchangeRate> rates)
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

        private double DetermineRevenueFor(ExchangeRate buyDateRate, ExchangeRate sellDateRate, double moneyUsd)
        {
            double initialValue = buyDateRate.value * moneyUsd / sellDateRate.value;
            double fees = (sellDateRate.date - buyDateRate.date).Days * _brokerFee;
            return initialValue - fees - moneyUsd;
        }
    }
}