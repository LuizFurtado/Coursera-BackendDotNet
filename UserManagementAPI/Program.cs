using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using UserManagementAPI.Models;
using UserManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UserService>();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        logger.LogError(exception, "Unhandled exception occurred.");

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\":\"An unexpected error occurred.\"}");
    });
});


app.MapGet("/users", (UserService service) =>
    TypedResults.Ok(service.GetAll()));

app.MapGet("/users/{userId}", Results<Ok<User>, NotFound> (Guid userId, UserService service) =>
{
    var user = service.GetById(userId);
    return user is not null ? TypedResults.Ok(user) : TypedResults.NotFound();
});

app.MapPost("/users", Results<Created<User>, BadRequest<string>> (
    User user, UserService service) =>
{
    var validationResults = new List<ValidationResult>();
    var context = new ValidationContext(user);

    if (!Validator.TryValidateObject(user, context, validationResults, true))
    {
        var errors = string.Join("; ", validationResults.Select(v => v.ErrorMessage));
        return TypedResults.BadRequest(errors);
    }

    service.Add(user);
    return TypedResults.Created($"/users/{user.UserId}", user);
});

app.MapPut("/users/{userId}", Results<Ok<User>, NotFound, Conflict, BadRequest<string>> (
    Guid userId, User updatedUser, UserService service) =>
{
    var validationResults = new List<ValidationResult>();
    var context = new ValidationContext(updatedUser);

    if (!Validator.TryValidateObject(updatedUser, context, validationResults, true))
    {
        var errors = string.Join("; ", validationResults.Select(v => v.ErrorMessage));
        return TypedResults.BadRequest(errors);
    }

    if (!service.Update(userId, updatedUser, out var error))
    {
        return error.Contains("conflict")
            ? TypedResults.Conflict()
            : TypedResults.NotFound();
    }

    return TypedResults.Ok(updatedUser);
});

app.MapDelete("/users/{userId}", Results<NoContent, NotFound> (Guid userId, UserService service) =>
{
    var success = service.Delete(userId);
    return success ? TypedResults.NoContent() : TypedResults.NotFound();
});

app.Run("http://localhost:5000");