using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Domain.Entities;
public class Role
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public Role(string name, string? description = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = true;
    }
    private Role()
    {
    }
}
