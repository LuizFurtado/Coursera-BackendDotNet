using System;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services;

public class UserService
{
  private readonly List<User> _users = new();
  private readonly object _lock = new();
  private readonly ILogger<UserService> _logger;

  public UserService(ILogger<UserService> logger)
  {
    _logger = logger;

    // Seed data
    _users.AddRange(new[]
    {
            new User { UserId = Guid.NewGuid(), Username = "alice", Email = "alice@example.com" },
            new User { UserId = Guid.NewGuid(), Username = "bob", Email = "bob@example.com" },
            new User { UserId = Guid.NewGuid(), Username = "charlie", Email = "charlie@example.com" }
        });

    _logger.LogInformation("UserService initialized with seed data.");
  }

  public IEnumerable<User> GetAll()
  {
    lock (_lock)
    {
      _logger.LogInformation("Fetching all users.");
      return _users.ToList();
    }
  }

  public User? GetById(Guid userId)
  {
    lock (_lock)
    {
      var user = _users.FirstOrDefault(u => u.UserId == userId);
      if (user is null)
        _logger.LogWarning("User with ID {UserId} not found.", userId);
      return user;
    }
  }

  public void Add(User user)
  {
    lock (_lock)
    {
      user.UserId = Guid.NewGuid();
      user.Version = 1;
      _users.Add(user);
      _logger.LogInformation("User {Username} added with ID {UserId}.", user.Username, user.UserId);
    }
  }

  public bool Update(Guid userId, User updatedUser, out string error)
  {
    lock (_lock)
    {
      var existingUser = _users.FirstOrDefault(u => u.UserId == userId);
      if (existingUser is null)
      {
        error = "User not found.";
        _logger.LogWarning("Update failed: user with ID {UserId} not found.", userId);
        return false;
      }

      if (existingUser.Version != updatedUser.Version)
      {
        error = "Concurrency conflict: user has been modified.";
        _logger.LogWarning("Concurrency conflict on user ID {UserId}. Expected version {Expected}, got {Actual}.",
            userId, existingUser.Version, updatedUser.Version);
        return false;
      }

      existingUser.Username = updatedUser.Username;
      existingUser.Email = updatedUser.Email;
      existingUser.Version++;
      error = string.Empty;

      _logger.LogInformation("User {UserId} updated successfully.", userId);
      return true;
    }
  }

  public bool Delete(Guid userId)
  {
    lock (_lock)
    {
      var user = _users.FirstOrDefault(u => u.UserId == userId);
      if (user is null)
      {
        _logger.LogWarning("Delete failed: user with ID {UserId} not found.", userId);
        return false;
      }

      _users.Remove(user);
      _logger.LogInformation("User {UserId} deleted.", userId);
      return true;
    }
  }
}
