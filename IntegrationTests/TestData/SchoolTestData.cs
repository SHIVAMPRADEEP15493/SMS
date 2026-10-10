using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Update;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.TestData;
public static class SchoolTestData
{
    public const string DefaultCity = "Noida";
    public const string DefaultState = "Uttar Pradesh";
    public const string DefaultCountry = "India";
    public const string DefaultPinCode = "201301";
    public static CreateSchoolMstRequest CreateRequest(string? code = null, string? name = null, string? email = null)
    {
        return new CreateSchoolMstRequest
        {
            SchoolCode = code ?? $"SCH-{Guid.NewGuid():N}",
            SchoolName = name ?? "Integration Test School",
            SchoolAddress = "Test Address",
            City = DefaultCity,
            State = DefaultState,
            Country = DefaultCountry,
            PinCode = DefaultPinCode,
            PhoneNumber = "9876543210",
            Email = email ?? "school@example.com"
        };
    }
    public static UpdateSchoolMstRequest CreateUpdateRequest(string? name = null)
    {
        return new UpdateSchoolMstRequest
        {
            SchoolName = name ?? "Updated Integration Test School",
            SchoolAddress = "Updated Address",
            City = DefaultCity,
            State = DefaultState,
            Country = DefaultCountry,
            PinCode = DefaultPinCode,
            PhoneNumber = "9999999999",
            Email = "updated@example.com"
        };
    }
    public static CreateSchoolMstRequest CreateRequestWithEmptyCode()
    {
        return CreateRequest(code: string.Empty);
    }
    public static CreateSchoolMstRequest CreateRequestWithInvalidEmail()
    {
        return CreateRequest(email: "invalid-email");
    }
    public static CreateSchoolMstRequest CreateRequestWithInvalidCode()
    {
        return CreateRequest(code: "SCH@001");
    }
}
