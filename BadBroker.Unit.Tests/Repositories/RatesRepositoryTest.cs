using BadBroker.Api.Models;
using BadBroker.Unit.Tests.MockServer;
using BadBroker.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Moq;
using System.Net.Http.Headers;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Hosting;

namespace BadBroker.Unit.Tests.Controllers;

public class RatesRepositoryTest
{
    private RatesRepository _repository;
    private TestServer _server;
    private ExchangeRateServerMock _exchangeRateServerMock = new ExchangeRateServerMock();

    [OneTimeSetUp]
    public void Init()
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("APIKEY", "APIKEY");
        _repository = new RatesRepository(_exchangeRateServerMock.Url, httpClient);
    }

    [Test]
    public async Task GivenTheExchangeService_WhenWeGetTheRates_WeGetTheRatesForRUBGBPEURJPY()
    {
        DateTime startDate = new DateTime(2014, 12, 15);
        DateTime endDate = new DateTime(2014, 12, 16);
        _exchangeRateServerMock.canGetExchangeRatesSuccessfullyFor(startDate);
        _exchangeRateServerMock.canGetExchangeRatesSuccessfullyFor(endDate);

        List<ExchangeRate> rates = await _repository.GetExchangeRatesFor(startDate, endDate);
        
        rates.Should().HaveCount(8);

        rates[0].date.Should().Be(15.December(2014));
        rates[0].currency.Should().Be(Currency.RUB);
        rates[0].value.Should().Be(1);

        rates[1].date.Should().Be(15.December(2014));
        rates[1].currency.Should().Be(Currency.EUR);
        rates[1].value.Should().Be(2);

        rates[2].date.Should().Be(15.December(2014));
        rates[2].currency.Should().Be(Currency.GBP);
        rates[2].value.Should().Be(3);

        rates[3].date.Should().Be(15.December(2014));
        rates[3].currency.Should().Be(Currency.JPY);
        rates[3].value.Should().Be(4);

        rates[4].date.Should().Be(16.December(2014));
        rates[4].currency.Should().Be(Currency.RUB);
        rates[4].value.Should().Be(1);

        rates[5].date.Should().Be(16.December(2014));
        rates[5].currency.Should().Be(Currency.EUR);
        rates[5].value.Should().Be(2);

        rates[6].date.Should().Be(16.December(2014));
        rates[6].currency.Should().Be(Currency.GBP);
        rates[6].value.Should().Be(3);

        rates[7].date.Should().Be(16.December(2014));
        rates[7].currency.Should().Be(Currency.JPY);
        rates[7].value.Should().Be(4);
    }
}
