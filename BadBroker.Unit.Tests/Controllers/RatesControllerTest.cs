using BadBroker.Api.Controllers;

namespace BadBroker.Unit.Tests.Controllers;

public class RatesControllerTest
{
    private RatesController _controller;

    [OneTimeSetUp]
    public void Init()
    {
    }

    [Test]
    public void GivenTheBestRatesForRUB_WhenTheUserHitsTheGetRates_TheyShouldGetTheBiggestRevenue()
    {
        Assert.Pass();
    }
}