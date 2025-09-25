var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
// Remove HTTPS reirection so you can test with HTTP
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();