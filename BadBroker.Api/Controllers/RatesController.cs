using BadBroker.Api.Exceptions;
using BadBroker.Api.Models;
using BadBroker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BadBroker.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RatesController : ControllerBase
{
    private IRatesService _ratesService;

    public RatesController(IRatesService ratesService)
    {
        _ratesService = ratesService;
    }

    [HttpGet("best")]
    public async Task<ActionResult<BestRatesResponse>> GetBestRatesFor([FromQuery(Name = "startDate")] DateTime startDate, [FromQuery(Name = "endDate")] DateTime endDate, [FromQuery(Name = "moneyUsd")] double moneyUsd)
    {
        if(endDate <= startDate)
        {
            return BadRequest("End date should be after start date");
        }

        if((endDate - startDate).Days > 60)
        {
            return BadRequest("Period can not exceed 60 days");
        }

        if(moneyUsd < 0) 
        {
            return BadRequest("Money can not be negative");
        }

        try 
        {
            BestRatesResponse response = await _ratesService.GetBestRatesFor(startDate, endDate, moneyUsd);
            return Ok(response);
        } catch (NoRevenueException)
        {
            return Conflict("Can't get revenue for the dates specified");
        } catch ( Exception )
        {
            return StatusCode(500);
        }
    }
}
