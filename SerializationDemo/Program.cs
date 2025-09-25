using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

public class SerializationDemo
{
  public class Person
  {
    public string UserName { get; set; }
    public int UserAge { get; set; }
  }

  static void Main()
  {
    var samplePerson = new Person { UserName = "John Doe", UserAge = 30 };

    using (FileStream fs = new FileStream("person.dat", FileMode.Create))
    {
      BinaryWriter writer = new BinaryWriter(fs);
      writer.Write(samplePerson.UserName);
      writer.Write(samplePerson.UserAge);
    }

    Console.WriteLine("Binary serialization complete.");

    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Person));
    using (StreamWriter writer = new StreamWriter("person.xml"))
    {
      xmlSerializer.Serialize(writer, samplePerson);
    }

    Console.WriteLine("XML serialization complete.");

    string jsonString = JsonSerializer.Serialize(samplePerson);
    File.WriteAllText("person.json", jsonString);

    Console.WriteLine("JSON serialization complete.");
  }
}
