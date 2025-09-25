using ClientApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

// Create the WebApplication builder
var builder = WebApplication.CreateBuilder(args);

// Add services for the API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Lab API v1"));

// --- ADDED: Explicitly configure routing middleware ---
app.UseRouting();
app.MapControllers();
// ---------------------------------------------------

// --- SERVER SETUP AND STARTUP ---
// We start the server in a non-blocking manner using RunAsync().
// This allows the code below to continue executing.
Console.WriteLine("Starting the ASP.NET Core server...");
var serverTask = Task.Run(() => app.RunAsync("http://localhost:5000"));

// Add a small delay to ensure the server has time to start listening.
// In a real-world scenario, you might use a retry loop or a health check.
await Task.Delay(3000);
Console.WriteLine("Server started. Attempting to connect with client...");

// --- CLIENT LOGIC ---
// This code will now execute AFTER the server is up and running.
try
{
  // Re-instantiate the generated client
  var httpClient = new HttpClient();
  var client = new GeneratedApiClient("http://localhost:5000", httpClient);

  // Call the generated method
  var user = await client.UserAsync(13);

  Console.WriteLine($"Successfully fetched user: {user.Name} with ID {user.UserId}");
}
catch (Exception ex)
{
  Console.WriteLine($"An error occurred while connecting to the server: {ex.Message}");

  // Check if the exception is an ApiException and provide more details
  if (ex is ClientApi.ApiException apiException)
  {
    Console.WriteLine($"API Exception Details: Status Code {apiException.StatusCode}");
    Console.WriteLine($"Response Body: {apiException.Response}");
  }
}

// Wait for the server to finish (if it ever does)
await serverTask;
