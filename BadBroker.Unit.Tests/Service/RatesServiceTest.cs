using BadBroker.Api.Models;
using BadBroker.Api.Services;
using BadBroker.Api.Repositories;
using FluentAssertions;
using Moq;

namespace BadBroker.Unit.Tests.Controllers;

public class RatesServiceTest
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
    public void GivenTheRatesForRUB_WhenWeGetTheBestRates_WeSholdGetARevenueOf27()
    {
        DateTime startDate = new DateTime(2014, 12, 15);
        DateTime endDate = new DateTime(2014, 12, 23);
        this.GivenThatThereAreRUBRates();

        BestRatesResponse response = _service.GetBestRatesFor(startDate, endDate, 100);
        
        response.rates.Should().HaveCount(9);
        validateRates(response.rates);
        response.buyDate.Should().Be(new DateTime(2014, 12, 16));
        response.sellDate.Should().Be(new DateTime(2014, 12, 22));
        response.tool.Should().Be(Currency.RUB);
        response.revenue.Should().BeApproximately(27.24, 0.01);
    }

    [Test]
    public void GivenThatThereAreFewRatesForRUB_WhenWeGetTheBestRates_WeSholdGetARevenueOf5()
    {
        DateTime startDate = new DateTime(2012, 01, 05);
        DateTime endDate = new DateTime(2012, 01, 07);
        this.GivenThatThereAreFewRUBRates();

        BestRatesResponse response = _service.GetBestRatesFor(startDate, endDate, 50);
        
        response.rates.Should().HaveCount(3);
        response.buyDate.Should().Be(new DateTime(2012, 01, 05));
        response.sellDate.Should().Be(new DateTime(2012, 01, 07));
        response.tool.Should().Be(Currency.RUB);
        response.revenue.Should().BeApproximately(5.14, 0.01);
    }

    private void GivenThatThereAreRUBRates()
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


        Mock.Get(_ratesRepository).Setup(s => s.GetExchangeRatesFor(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(rates);
    }

    private void GivenThatThereAreFewRUBRates()
    {
        List<ExchangeRate> rates = new List<ExchangeRate>();
        
        ExchangeRate rate5 = new ExchangeRate();
        rate5.date = new DateTime(2012, 01, 05);
        rate5.value = 40;
        rate5.currency = Currency.RUB;
        rates.Add(rate5);

        ExchangeRate rate8 = new ExchangeRate();
        rate8.date = new DateTime(2012, 01, 07);
        rate8.value = 35;
        rate8.currency = Currency.RUB;
        rates.Add(rate8);

        ExchangeRate rate19 = new ExchangeRate();
        rate19.date = new DateTime(2012, 01, 19);
        rate19.value = 30;
        rate19.currency = Currency.RUB;
        rates.Add(rate19);


        Mock.Get(_ratesRepository).Setup(s => s.GetExchangeRatesFor(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(rates);
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
    
}
