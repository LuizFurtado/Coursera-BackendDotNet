using System;

namespace UserManagementAPI.Models;

using System.ComponentModel.DataAnnotations;

public class User
{
  public Guid UserId { get; set; } = Guid.NewGuid();

  [Required]
  public string Username { get; set; } = string.Empty;

  [Required]
  [EmailAddress]
  public string Email { get; set; } = string.Empty;

  public int Version { get; set; } = 1; // Used for concurrency control
}
