using Microsoft.VisualBasic;
using SimpleCrudApp.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// In memory list of Item
var items = new List<Item>
{
    new Item { Id = 1, Name= "Item 1" },
    new Item { Id = 2, Name= "Item 2" },
    new Item { Id = 3, Name= "Item 3" },
    new Item { Id = 4, Name= "Item 4" },
    new Item { Id = 5, Name= "Item 5" },
};

// Basic Routes
app.MapGet("/", () => "Welcome to my Simple Web Api");
app.MapGet("/Items", () => Results.Ok(items));
app.MapGet("/Items/{id}", (int id) =>
{
    var item = items.Find(x => x.Id == id);
    return item is null ? Results.NotFound() : Results.Ok(item);
});
app.MapPost("/Items", (Item item) =>
{
    var newItem = item;
    items.Add(newItem);
    return Results.Created($"/Items/{newItem.Id}", newItem);
});
app.MapPut("/Items/{id}", (int id, Item item) =>
{
    var existingItem = items.Find(x => x.Id == id);
    if (existingItem is null) return Results.NotFound();
    existingItem.Name = item.Name;
    return Results.Ok(existingItem);
});
app.MapDelete("/Items/{id}", (int id) =>
{
    var existingItem = items.Find(x => x.Id == id);
    if (existingItem is null) return Results.NotFound();
    items.Remove(existingItem);
    return Results.NoContent();
});
app.Run();