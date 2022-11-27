using BadBroker.Api.Models;
using BadBroker.Api.Services;
using BadBroker.Api.Repositories;
using FluentAssertions;
using Moq;

namespace BadBroker.Unit.Tests.Controllers;

public class RatesServiceest
{
    private RatesService _service;
    private IRatesRepository _ratesRepository;

    [OneTimeSetUp]
    public void Init()
    {
        _ratesRepository = Mock.Of<IRatesRepository>();
        _service = new RatesService(_ratesRepository);
    }

    [Test]
    public void GivenThRatesForRUB_WhenWeGetTheBestRates_WeSholdGetARevenueOf127()
    {
        DateTime startDate = new DateTime(2014, 12, 15);
        DateTime endDate = new DateTime(2014, 12, 23);
        this.GivenThatThereAreRUBRatesFor(startDate, endDate);

        BestRatesResponse response = _service.GetBestRatesFor(startDate, endDate, 100);
        
        response.rates.Should().HaveCount(9);
        validateRates(response.rates);
        response.buyDate.Should().Be(new DateTime(2014, 12, 16));
        response.sellDate.Should().Be(new DateTime(2014, 12, 22));
        response.tool.Should().Be(Currency.RUB);
        response.revenue.Should().BeApproximately(27.24, 0.01);
    }

    private void validateRates(IEnumerable<RateDto> rates)
    {
        rates.ToList()[0].rub.Should().BeApproximately(60.17, 0.01);
        rates.ToList()[1].rub.Should().BeApproximately(72.99, 0.01);
        rates.ToList()[2].rub.Should().BeApproximately(66.01, 0.01);
        rates.ToList()[3].rub.Should().BeApproximately(61.44, 0.01);
        rates.ToList()[4].rub.Should().BeApproximately(59.79, 0.01);
        rates.ToList()[5].rub.Should().BeApproximately(59.79, 0.01);
        rates.ToList()[6].rub.Should().BeApproximately(59.79, 0.01);
        rates.ToList()[7].rub.Should().BeApproximately(54.78, 0.01);
        rates.ToList()[8].rub.Should().BeApproximately(54.80, 0.01);
    }

    private void GivenThatThereAreRUBRatesFor(DateTime startDate, DateTime endDate)
    {
        List<ExchangeRate> rates = new List<ExchangeRate>();
        
        ExchangeRate rate15 = new ExchangeRate();
        rate15.date = new DateTime(2014, 12, 15);
        rate15.value = 60.17;
        rate15.currency = Currency.RUB;
        rates.Add(rate15);

        ExchangeRate rate16 = new ExchangeRate();
        rate16.date = new DateTime(2014, 12, 16);
        rate16.value = 72.99;
        rate16.currency = Currency.RUB;
        rates.Add(rate16);

        ExchangeRate rate17 = new ExchangeRate();
        rate17.date = new DateTime(2014, 12, 17);
        rate17.value = 66.01;
        rate17.currency = Currency.RUB;
        rates.Add(rate17);

        ExchangeRate rate18 = new ExchangeRate();
        rate18.date = new DateTime(2014, 12, 18);
        rate18.value = 61.44;
        rate18.currency = Currency.RUB;
        rates.Add(rate18);

        ExchangeRate rate19 = new ExchangeRate();
        rate19.date = new DateTime(2014, 12, 19);
        rate19.value = 59.79;
        rate19.currency = Currency.RUB;
        rates.Add(rate19);

        ExchangeRate rate20 = new ExchangeRate();
        rate20.date = new DateTime(2014, 12, 20);
        rate20.value = 59.79;
        rate20.currency = Currency.RUB;
        rates.Add(rate20);

        ExchangeRate rate21 = new ExchangeRate();
        rate21.date = new DateTime(2014, 12, 21);
        rate21.value = 59.79;
        rate21.currency = Currency.RUB;
        rates.Add(rate21);

        ExchangeRate rate22 = new ExchangeRate();
        rate22.date = new DateTime(2014, 12, 22);
        rate22.value = 54.78;
        rate22.currency = Currency.RUB;
        rates.Add(rate22);

        ExchangeRate rate23 = new ExchangeRate();
        rate23.date = new DateTime(2014, 12, 23);
        rate23.value = 54.80;
        rate23.currency = Currency.RUB;
        rates.Add(rate23);


        Mock.Get(_ratesRepository).Setup(s => s.GetExchangeRatesFor(startDate, endDate)).Returns(rates);
    }    
}

/*
Date RUB/USD
2014-12-15 60.17
2014-12-16 72.99
2014-12-17 66.01
2014-12-18 61.44
2014-12-19 59.79
2014-12-20 59.79
2014-12-21 59.79
2014-12-22 54.78
2014-12-23 54.80
You have $100 and the specified period is December 2014. The best case for this period is RUB for
12/16/2014-12/22/2014 . If you bought RUB on 12/16/2014 and sell them 12/22/2014 you would get ~$127. So
revenue is $27.
(72.99 * 100 / 54.78) - 6 (days broker fee) = $127.24.
*/