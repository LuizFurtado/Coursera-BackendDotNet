using System;

namespace UserManagementAPI.Middlewares;

public class TokenAuthenticationMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<TokenAuthenticationMiddleware> _logger;
  private const string AUTH_HEADER = "Authorization";
  private const string BEARER_PREFIX = "Bearer ";

  // Replace with your actual token store or validation logic
  private static readonly HashSet<string> ValidTokens = new()
    {
        "token123", "securetoken456", "adminToken789"
    };

  public TokenAuthenticationMiddleware(RequestDelegate next, ILogger<TokenAuthenticationMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    var path = context.Request.Path;
    var method = context.Request.Method;

    // Skip authentication for root or health check if needed
    if (path == "/" || path.StartsWithSegments("/health"))
    {
      await _next(context);
      return;
    }

    if (!context.Request.Headers.TryGetValue(AUTH_HEADER, out var authHeader) ||
        !authHeader.ToString().StartsWith(BEARER_PREFIX))
    {
      _logger.LogWarning("Unauthorized request: missing or malformed token. Method: {Method}, Path: {Path}", method, path);
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      await context.Response.WriteAsync("Unauthorized: Missing or malformed token.");
      return;
    }

    var token = authHeader.ToString()[BEARER_PREFIX.Length..];

    if (!ValidTokens.Contains(token))
    {
      _logger.LogWarning("Unauthorized request: invalid token. Method: {Method}, Path: {Path}", method, path);
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      await context.Response.WriteAsync("Unauthorized: Invalid token.");
      return;
    }

    _logger.LogInformation("Authenticated request. Method: {Method}, Path: {Path}", method, path);
    await _next(context);
  }
}
