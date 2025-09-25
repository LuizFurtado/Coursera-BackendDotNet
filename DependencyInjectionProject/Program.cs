using DependencyInjectionProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Services for different lifetimes
// builder.Services.AddSingleton<IMyService, MyService>();
// builder.Services.AddScoped<IMyService, MyService>();
builder.Services.AddTransient<IMyService, MyService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var service = context.RequestServices.GetRequiredService<IMyService>();
    service.LogCreation("Middleware 1");
    await next();
});

app.Use(async (context, next) =>
{
    var service = context.RequestServices.GetRequiredService<IMyService>();
    service.LogCreation("Middleware 2");
    await next();
});

app.MapGet("/", (IMyService service) =>
{
    service.LogCreation("Root");
    return Results.Ok("Check the console for service creation logs");
});

app.Run();
