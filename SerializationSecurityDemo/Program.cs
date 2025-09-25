using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public class SerializationSecurityDemo
{
  public class User
  {
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public string GenerateHash()
    {
      using (SHA256 sha256 = SHA256.Create())
      {
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(ToString()));
        return Convert.ToBase64String(hashBytes);
      }
    }

    public void EncryptData()
    {
      Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(Password));
    }

    public static User DeserializeUserData(string jsondata, bool isTrustedSource)
    {
      if (!isTrustedSource)
      {
        Console.WriteLine("Deserialization blocked: Untrusted source.");
        return null;
      }

      return JsonSerializer.Deserialize<User>(jsondata);
    }

    public static string SerializeUserData(User user)
    {
      if (string.IsNullOrWhiteSpace(user.Name)
        || string.IsNullOrWhiteSpace(user.Email)
        || string.IsNullOrWhiteSpace(user.Password))
      {
        Console.WriteLine("Invalid data. Serialization aborted.");
        return string.Empty;
      }

      user.EncryptData();
      return JsonSerializer.Serialize(user);
    }

    public override string ToString() => JsonSerializer.Serialize(this);
  }

  public static void Main(string[] args)
  {
    User user = new User
    {
      Name = "John Doe",
      Email = "lYV5t@example.com",
      Password = "password123"
    };

    string generatedHash = user.GenerateHash();
    string serializedData = User.SerializeUserData(user);
    Console.WriteLine("Serialized data:\n" + serializedData);
    Console.WriteLine("Generated hash:\n" + generatedHash);
  }
}