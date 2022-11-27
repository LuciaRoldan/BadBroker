using BadBroker.Api.Controllers;
using BadBroker.Api.Models;
using BadBroker.Api.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BadBroker.Unit.Tests.Controllers;

public class RatesControllerTest
{
    private RatesController _controller;
    private ILogger<RatesController> _logger;
    private IRatesService _ratesService;

    [OneTimeSetUp]
    public void Init()
    {
        _logger = Mock.Of<ILogger<RatesController>>();
        _ratesService = Mock.Of<IRatesService>();
        _controller = new RatesController(_logger, _ratesService);
    }

    [Test]
    public async Task GivenTheBestRatesForRUB_WhenTheUserHitsTheGetRates_TheyShouldGetTheBiggestRevenue()
    {
        DateTime startDate = new DateTime(2014, 12, 15);
        DateTime endDate = new DateTime(2014, 12, 23);
        this.GivenThatThereAreRatesFor(startDate, endDate);

        BestRatesResponse response = await _controller.GetBestRatesFor(startDate, endDate, 100);
        
        response.rates.Should().HaveCount(9);
        for (int i = 0; i < response.rates.Count(); i++)
        {
            //Note: the values in here are mocked for easy testing but do not reflect how the algorithm 
            //for calculating the best rate works. That is tested on the RatesServiceTest

            RateDto rate = response.rates.ToList()[i];
            rate.date.Should().Be(startDate.AddDays(i));
            rate.rub = i;
            rate.eur = i;
            rate.gbp = i;
            rate.jpy = i;
        }
        response.buyDate.Should().Be(new DateTime(2014, 12, 16));
        response.sellDate.Should().Be(new DateTime(2014, 12, 22));
        response.tool.Should().Be(Currency.RUB);
        response.revenue.Should().Be(27.258783297622983);
    }

    private void GivenThatThereAreRatesFor(DateTime startDate, DateTime endDate)
    {
        List<RateDto> rates = new List<RateDto>();
        for (int i = 0; i <= (endDate - startDate).TotalDays; i++)
        {
            //Note: the values in here are mocked for easy testing but do not reflect how the algorithm 
            //for calculating the best rate works. That is tested on the RatesServiceTest

            RateDto rate = new RateDto();
            rate.date = startDate.AddDays(i);
            rate.rub = i;
            rate.eur = i;
            rate.gbp = i;
            rate.jpy = i;

            rates.Add(rate);
        }

        BestRatesResponse mockedResponse = new BestRatesResponse();
        mockedResponse.rates = rates;
        mockedResponse.buyDate = new DateTime(2014, 12, 16);
        mockedResponse.sellDate = new DateTime(2014, 12, 22);
        mockedResponse.tool = Currency.RUB;
        mockedResponse.revenue = 27.258783297622983;

        Mock.Get(_ratesService).Setup(s => s.GetBestRatesFor(startDate, endDate, 100)).ReturnsAsync(mockedResponse);
    }
    
}