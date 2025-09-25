using SwaggerClientGeneratorDemo;
using BlogApi;

var httpClient = new HttpClient();
var apiBaseUrl = "http://localhost:5072";

var client = new BlogApiClient(apiBaseUrl, httpClient);

var blogs = await client.BlogsAllAsync();

if (blogs != null)
{
  foreach (var blog in blogs)
  {
    Console.WriteLine($"Title: {blog.Title}, Body: {blog.Body}");
  }
}

// Will run it only once to generate the client
// and only after when there are changes to the
// API definition to generate the client
//var clientGenerator = new ClientGenerator();
// await clientGenerator.GenerateClient();

// var httpResults = await httpClient.GetAsync($"{apiBaseUrl}/blogs");

// if (httpResults.StatusCode != System.Net.HttpStatusCode.OK)
// {
//   Console.WriteLine("Failed to fetch data from the API.");
//   return;
// }

// var blogStream = await httpResults.Content.ReadAsStreamAsync();

// var options = new System.Text.Json.JsonSerializerOptions
// {
//   PropertyNameCaseInsensitive = true
// };

// var blogs = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Blog>>(blogStream, options);

// if (blogs != null)
// {
//   foreach (var blog in blogs)
//   {
//     Console.WriteLine($"Title: {blog.Title}, Body: {blog.Body}");
//   }
// }


class Blog
{
  public required string Title { get; set; }
  public required string Body { get; set; }
}
