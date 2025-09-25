var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpLogging();

app.MapGet("/", () => "Hello, ASP .NET Core Middleware");

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request path: {context.Request.Path}");
    await next();
    Console.WriteLine($"Response status code: {context.Response.StatusCode}");
});

app.Use(async (context, next) =>
{
    var startTime = DateTime.Now;
    Console.WriteLine($"Start time: {startTime}");
    await next();
    var duration = DateTime.Now - startTime;
    Console.WriteLine($"Response time: {duration.TotalMilliseconds} ms");
});

app.Run();