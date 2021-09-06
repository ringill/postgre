using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;

namespace iug.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : Controller
  {
    private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private readonly ILogger<WeatherForecastController> _logger;
    private ISession session;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ISession session)
    {
      _logger = logger;
      this.session = session;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        session = null;
      }
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
      var rng = new Random();
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = Summaries[rng.Next(Summaries.Length)]
      })
      .ToArray();
    }

    [HttpGet]
    [Route("/book")]
    public async Task<IActionResult> Book()
    {
      try
      {
        var books = await session.Query<object>().ToListAsync();

        return Ok(books);
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }
  }
}
