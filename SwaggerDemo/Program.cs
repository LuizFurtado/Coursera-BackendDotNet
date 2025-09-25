using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var blogs = new List<Blog>
{
    new() { Title = "Blog 1", Body = "Content 1" },
    new() { Title = "Blog 2", Body = "Content 2" },
    new() { Title = "Blog 3", Body = "Content 3" }
};

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "I'm root!").ExcludeFromDescription();

app.MapGet("/blogs", () => blogs);

app.MapGet("/blogs/{id}", Results<Ok<Blog>, NotFound> (int id) =>
{
    if (id < 0 || id >= blogs.Count)
    {
        return TypedResults.NotFound();
    }
    else
    {
        return TypedResults.Ok(blogs[id]);
    }
}).WithOpenApi(operation =>
{
    operation.Parameters[0].Description = "Id of a single blog post";
    operation.Summary = "Get a single blog post";
    operation.Description = "Returns a single blog post from its id";
    return operation;
});

app.MapPost("/blogs", (Blog blog) =>
{
    blogs.Add(blog);
    return TypedResults.Created($"/blogs/{blogs.Count - 1}", blog);
});

app.MapDelete("/blogs/{id}", Results<NotFound, NoContent> (int id) =>
{
    if (id < 0 || id >= blogs.Count)
    {
        return TypedResults.NotFound();
    }
    else
    {
        var blog = blogs[id];
        blogs.RemoveAt(id);
        return TypedResults.NoContent();
    }
});

app.MapPut("/blogs/{id}", Results<NotFound, NoContent> (int id, Blog blog) =>
{
    if (id < 0 || id >= blogs.Count)
    {
        return TypedResults.NotFound();
    }
    else
    {
        blogs[id] = blog;
        return TypedResults.NoContent();
    }
});

app.Run();

public class Blog
{
    public required string Title { get; set; }
    public required string Body { get; set; }
}