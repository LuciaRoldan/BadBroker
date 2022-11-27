using BadBroker.Api.Models;
using BadBroker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadBroker.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RatesController : ControllerBase
{
    private readonly ILogger<RatesController> _logger;
    private IRatesService _ratesService;

    public RatesController(ILogger<RatesController> logger, IRatesService ratesService)
    {
        _logger = logger;
        _ratesService = ratesService;
    }

    [HttpGet("best")]
    public async Task<BestRatesResponse> GetBestRatesFor([FromQuery(Name = "startDate")] DateTime startDate, [FromQuery(Name = "endDate")] DateTime endDate, [FromQuery(Name = "moneyUsd")] double moneyUsd)
    {
        return await _ratesService.GetBestRatesFor(startDate, endDate, moneyUsd);
    }
}
