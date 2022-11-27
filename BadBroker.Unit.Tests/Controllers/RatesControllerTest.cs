using BadBroker.Api.Controllers;
using BadBroker.Api.Models;
using BadBroker.Api.Services;
using BadBroker.Api.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BadBroker.Unit.Tests.Controllers;

public class RatesControllerTest
{
    private RatesController _controller;
    private IRatesService _ratesService;

    [OneTimeSetUp]
    public void Init()
    {
        _ratesService = Mock.Of<IRatesService>();
        _controller = new RatesController(_ratesService);
    }

    [Test]
    public async Task GivenTheBestRatesForRUB_WhenTheUserHitsTheGetRates_TheyShouldGetTheBiggestRevenue()
    {
        DateTime startDate = new DateTime(2014, 12, 15);
        DateTime endDate = new DateTime(2014, 12, 23);
        this.GivenThatThereAreRatesFor(startDate, endDate);

        var response = await _controller.GetBestRatesFor(startDate, endDate, 100);
        var result = response.Result as ObjectResult;
        result.StatusCode.Should().Be(200);

        BestRatesResponse bestRatesResponse = result.Value as BestRatesResponse;
        
        bestRatesResponse.Should().NotBeNull();
        bestRatesResponse.rates.Should().HaveCount(9);
        for (int i = 0; i < bestRatesResponse.rates.Count(); i++)
        {
            //Note: the values in here are mocked for easy testing but do not reflect how the algorithm 
            //for calculating the best rate works. That is tested on the RatesServiceTest

            RateDto rate = bestRatesResponse.rates.ToList()[i];
            rate.date.Should().Be(startDate.AddDays(i));
            rate.rub = i;
            rate.eur = i;
            rate.gbp = i;
            rate.jpy = i;
        }
        bestRatesResponse.buyDate.Should().Be(new DateTime(2014, 12, 16));
        bestRatesResponse.sellDate.Should().Be(new DateTime(2014, 12, 22));
        bestRatesResponse.tool.Should().Be(Currency.RUB);
        bestRatesResponse.revenue.Should().Be(27.258783297622983);
    }

    [Test]
    public async Task GivenThatTheValuesAreIncorrect_WhenTheUserHitsTheGetRates_TheyShouldGetABadRequest()
    {
        DateTime startDate = new DateTime(2014, 12, 15);
        DateTime endDate = new DateTime(2013, 12, 23);

        ActionResult<BestRatesResponse> response = await _controller.GetBestRatesFor(startDate, endDate, 100);
        
        response.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task GivenThatTheDatesAreMoreThan2MonthsApart_WhenTheUserHitsTheGetRates_TheyShouldGetABadRequest()
    {
        DateTime startDate = new DateTime(2012, 01, 01);
        DateTime endDate = startDate.AddDays(61);

        ActionResult<BestRatesResponse> response = await _controller.GetBestRatesFor(startDate, endDate, 100);
        
        response.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task GivenThatTheMoneyIsLowerThan0_WhenTheUserHitsTheGetRates_TheyShouldGetABadRequest()
    {
        DateTime startDate = new DateTime(2012, 01, 01);
        DateTime endDate = startDate.AddDays(10);

        ActionResult<BestRatesResponse> response = await _controller.GetBestRatesFor(startDate, endDate, -100);
        
        response.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task GivenThatTAnErrorOcurrs_WhenTheUserHitsTheGetRates_TheyShouldGetAnInternalServerError()
    {
        DateTime startDate = new DateTime(2012, 01, 01);
        DateTime endDate = startDate.AddDays(10);
        GivenThatTheServiceThrowsAnException();

        var response = await _controller.GetBestRatesFor(startDate, endDate, 100);
        var result = response.Result as StatusCodeResult;

        result.StatusCode.Should().Be(500);
    }

    [Test]
    public async Task GivenThatThereIsNoRevenue_WhenTheUserHitsTheGetRates_TheyShouldGetAnInternalServerError()
    {
        DateTime startDate = new DateTime(2012, 01, 01);
        DateTime endDate = startDate.AddDays(10);
        GivenThatTheServiceThrowsANoRevenueException();

        ActionResult<BestRatesResponse> response = await _controller.GetBestRatesFor(startDate, endDate, 100);
        
        response.Result.Should().BeOfType<ConflictObjectResult>();
    }

    private void GivenThatTheServiceThrowsANoRevenueException()
    {
        Mock.Get(_ratesService).Setup(s => s.GetBestRatesFor(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<double>())).Throws(new NoRevenueException());
    }

    private void GivenThatTheServiceThrowsAnException()
    {
        Mock.Get(_ratesService).Setup(s => s.GetBestRatesFor(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<double>())).Throws(new Exception());
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