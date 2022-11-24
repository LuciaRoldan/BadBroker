using Microsoft.AspNetCore.Mvc;

namespace BadBroker.Api.Controllers;

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
    public List<string> GetBestRatesFor()
    {
        return new List<string>();
    }
}
