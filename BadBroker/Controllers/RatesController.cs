using Microsoft.AspNetCore.Mvc;

namespace BadBroker.Controllers;

[ApiController]
[Route("[controller]")]
public class RatesController : ControllerBase
{
    private readonly ILogger<RatesController> _logger;

    public RatesController(ILogger<RatesController> logger)
    {
        _logger = logger;
    }

    [HttpGet("best")]
    public List<WeatherForecast> GetBestRatesFor()
    {
        return new List<WeatherForecast>();
    }
}
