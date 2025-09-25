using Newtonsoft.Json;

namespace JsonExample
{
  public class Person
  {
    public string Name { get; set; }
    public int Age { get; set; }
  }
  public class Program
  {
    public static void Main(string[] args)
    {
      var json = "{\"Name\":\"John Doe\",\"Age\":30}";
      Person person = JsonConvert.DeserializeObject<Person>(json);
      Console.WriteLine($"Deserealized Person: Name: {person.Name}, Age: {person.Age}");

      Person person2 = new()
      {
        Name = "Jane Doe",
        Age = 25
      };
      string json2 = JsonConvert.SerializeObject(person2);
      Console.WriteLine($"Serealized Person: {json2}");
    }
  }
}