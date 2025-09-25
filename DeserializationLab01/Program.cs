using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

public class DeserializationLab01
{
  public class Person
  {
    public string UserName { get; set; }
    public int UserAge { get; set; }
  }

  static void Main()
  {
    // Serialize example for testing
    var samplePerson = new Person { UserName = "Alice", UserAge = 30 };

    // Binary serialization
    using (var fs = new FileStream("person.dat", FileMode.Create))
    using (var writer = new BinaryWriter(fs))
    {
      writer.Write(samplePerson.UserName);
      writer.Write(samplePerson.UserAge);
    }
    Console.WriteLine("Binary serialization complete.");

    Stopwatch stopwatch = Stopwatch.StartNew();

    using (var fs = new FileStream("person.dat", FileMode.Open))
    using (var reader = new BinaryReader(fs))
    {
      var deserializedPerson = new Person()
      {
        UserName = reader.ReadString(),
        UserAge = reader.ReadInt32(),
      };

      stopwatch.Stop();
      Console.WriteLine($"Binary deserialization took {stopwatch.ElapsedMilliseconds} ms. Name: {deserializedPerson.UserName}, Age: {deserializedPerson.UserAge}");
    }

    var xmlData = "<Person><UserName>John Doe</UserName><UserAge>30</UserAge></Person>";
    var serializer = new XmlSerializer(typeof(Person));

    Stopwatch stopwatch1 = Stopwatch.StartNew();

    using (var reader = new StringReader(xmlData))
    {
      var deserializedPerson = (Person)serializer.Deserialize(reader);
      stopwatch1.Stop();

      Console.WriteLine($"Xml deserialization took {stopwatch1.ElapsedMilliseconds} ms. Name: {deserializedPerson.UserName}, Age: {deserializedPerson.UserAge}");
    }

    var jsonData = "{\"UserName\":\"Alice\",\"UserAge\":45}";

    Stopwatch stopwatch2 = Stopwatch.StartNew();

    var deserializedPerson2 = JsonSerializer.Deserialize<Person>(jsonData);
    stopwatch2.Stop();

    Console.WriteLine($"Json deserialization took {stopwatch2.ElapsedMilliseconds} ms. Name: {deserializedPerson2.UserName}, Age: {deserializedPerson2.UserAge}");
  }
}