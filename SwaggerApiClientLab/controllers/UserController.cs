using System;
using Microsoft.AspNetCore.Mvc;

namespace SwaggerApiClientLab.controllers;

public class User
{
  public int UserId { get; set; }
  public required string Name { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
  [HttpGet("{id}")]
  [Produces("application/json")]
  public ActionResult<User> GetUser(int id)
  {
    var user = new User
    {
      UserId = id,
      Name = $"User {id}"
    };

    return Ok(user);
  }
}
