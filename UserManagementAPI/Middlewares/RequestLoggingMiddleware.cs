using System;

namespace UserManagementAPI.Middlewares;

public class RequestLoggingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<RequestLoggingMiddleware> _logger;

  public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    var method = context.Request.Method;
    var path = context.Request.Path;

    // Capture the original response body
    var originalBodyStream = context.Response.Body;
    using var responseBody = new MemoryStream();
    context.Response.Body = responseBody;

    await _next(context); // Proceed to next middleware

    // Read status code after response is written
    var statusCode = context.Response.StatusCode;

    _logger.LogInformation("HTTP {Method} {Path} responded {StatusCode}",
        method, path, statusCode);

    // Copy response back to original stream
    responseBody.Seek(0, SeekOrigin.Begin);
    await responseBody.CopyToAsync(originalBodyStream);
  }
}
