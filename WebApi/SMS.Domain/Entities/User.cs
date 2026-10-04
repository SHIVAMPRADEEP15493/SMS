using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Domain.Entities;
public class User
{
    public Guid Id { get; private set; }
    public string UserName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    private User()
    {
    }
    public User(string userName, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = true;
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
    }
}
