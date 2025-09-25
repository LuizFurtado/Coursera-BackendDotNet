using System;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApp;

public class WeatherForecast
{
  public DateTime Date { get; set; }
  public int Temperature { get; set; }
  public string? Summary { get; set; }
}

[ApiController]
[Route("[controller]")]
public class WeathForecastController : ControllerBase
{
  private static readonly string[] Summaries = new[]
  {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
  };

  [HttpGet]
  public IEnumerable<WeatherForecast> Get()
  {
    var rng = new Random();
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
      Date = DateTime.Now.AddDays(index),
      Temperature = rng.Next(-20, 55),
      Summary = Summaries[rng.Next(Summaries.Length)]
    }).ToArray();
  }

  [HttpPost]
  public IActionResult Post([FromBody] WeatherForecast forecast)
  {
    // Add to storage(e.g. database)
    return Ok(forecast);
  }

  [HttpPut]
  public IActionResult Put(int id, [FromBody] WeatherForecast forecast)
  {
    // Update data for the given Id
    // Example: Find and update an item with match ID
    // var existingForecast = /* fetch data */;
    // existingForecast.Date = forecast.Date;
    return NoContent();
  }

  [HttpDelete]
  public IActionResult Delete(int id)
  {
    // Delete data for the given Id
    return NoContent();
  }
}
