using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Domain.Entities;
public sealed class SchoolMst
{
    /*
    public Guid Id { get; private set; }

    public string SchoolCode { get; private set; } = null!;

    public string SchoolName { get; private set; } = null!;

    public string? SchoolAddress { get; private set; }

    public string? City { get; private set; }

    public string? State { get; private set; }
    public string? Country { get; private set; }

    public string? PinCode { get; private set; }

    public string? PhoneNumber { get; private set; }

    public string? Email { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    private SchoolMst()
    {
    }

    public SchoolMst(
        string code,
        string name)
    {
        Id = Guid.NewGuid();

        SchoolCode = code;
        SchoolName = name;

        IsActive = true;

        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string name,
        string? address,
        string? city,
        string? state,
        string? pinCode,
        string? phoneNumber,
        string? email)
    {
        SchoolName = name;
        SchoolAddress = address;
        City = city;
        State = state;
        PinCode = pinCode;
        PhoneNumber = phoneNumber;
        Email = email;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
    */

    public long Id { get; private set; }
    public string SchoolCode { get; private set; } = null!;
    public string SchoolName { get; private set; } = null!;
    public string? SchoolAddress { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? Country { get; private set; }
    public string? PinCode { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? Email { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    private SchoolMst()
    {
    }

    public SchoolMst(
        string code,
        string name,
        string? address,
        string? city,
        string? state,
        string? country,
        string? pinCode,
        string? phoneNumber,
        string? email)
    {
        Id = 0; // Will be set by the database

        SchoolCode = code;
        SchoolName = name;
        SchoolAddress = address;
        City = city;
        State = state;
        Country = country;
        PinCode = pinCode;
        PhoneNumber = phoneNumber;
        Email = email;

        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string schoolName,
        string? schoolAddress,
        string? city,
        string? state,
        string? country,
        string? pinCode,
        string? phoneNumber,
        string? email)
    {
        SchoolName = schoolName;
        SchoolAddress = schoolAddress;
        City = city;
        State = state;
        Country = country;
        PinCode = pinCode;
        PhoneNumber = phoneNumber;
        Email = email;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}

