using Microsoft.AspNetCore.Mvc;

namespace LoggingProjectLab.Controllers;

[Controller]
[Route("api/[controller]")]
public class ErrorHandlingController : ControllerBase
{
  [HttpGet("division")]
  public ActionResult GetDivisionResult(int numerator, int denominator)
  {
    try
    {
      return Ok(numerator / denominator);
    }
    catch (DivideByZeroException ex)
    {
      Console.WriteLine("Error: Division by zero is not allowed.");
      return BadRequest($"Error: you cannot divide by zero: {ex.Message}");
    }
  }
}
