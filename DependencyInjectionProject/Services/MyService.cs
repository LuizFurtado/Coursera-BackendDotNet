namespace DependencyInjectionProject.Services;

public class MyService : IMyService
{
  private int _serviceId;

  public MyService()
  {
    _serviceId = new Random().Next(1000000, 9999999);
  }
  public void LogCreation(string message)
  {
    Console.WriteLine($"Service Id: {_serviceId} - {message}");
  }
}
