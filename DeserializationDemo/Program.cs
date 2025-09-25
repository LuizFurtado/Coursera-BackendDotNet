using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "I am Root");

app.MapPost("/auto", (Person personfromClient) =>
{
    return TypedResults.Ok(personfromClient);
});

app.MapPost("/json", async (HttpContext context) =>
{
    var person = await context.Request.ReadFromJsonAsync<Person>();
    return TypedResults.Json(person);
});

app.MapPost("/custom-options", async (HttpContext context) =>
{
    var options = new JsonSerializerOptions
    {
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    };
    var person = await context.Request.ReadFromJsonAsync<Person>(options);
    return TypedResults.Json(person);
});

app.MapPost("/xml", async (HttpContext context) =>
{
    var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    var xmlSerializer = new XmlSerializer(typeof(Person));
    var stringReader = new StringReader(body);
    var person = xmlSerializer.Deserialize(stringReader);
    return TypedResults.Ok(person);
});

app.Run();

public class Person
{
    required public string Name { get; set; }
    public int? Age { get; set; }
}