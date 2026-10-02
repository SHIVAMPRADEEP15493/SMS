using IntegrationTests.Infrastructure;
using SMS.Application.SchoolsMst.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace IntegrationTests.Schools;

[Collection("Integration Tests")]
public class SchoolsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    //private readonly HttpClient _client;

    //public SchoolsControllerTests(CustomWebApplicationFactory factory)
    //{
    //    _client = factory.CreateClient();
    //}
    private readonly CustomWebApplicationFactory _factory;

    private readonly HttpClient _client;

    public SchoolsControllerTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    private async Task ResetDatabaseAsync()
    {
        await _factory.ResetDatabaseAsync();
    }
    [Fact]
    public async Task Create_WithValidRequest_ShouldReturnCreated()
    {
        await ResetDatabaseAsync();
        // Arrange
        var request = new CreateSchoolMstRequest
        {
            SchoolCode = $"TEST-{Guid.NewGuid():N}",
            SchoolName = "Integration Test School",
            SchoolAddress = "Test Address",
            City = "Noida",
            State = "Uttar Pradesh",
            PinCode = "201301",
            Country = "India",
            PhoneNumber = "9876543210",
            Email = "test@example.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Schools",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        await ResetDatabaseAsync();
        // Act
        var response = await _client.GetAsync("/api/Schools");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WhenSchoolExists_ShouldReturnOk()
    {
        // Arrange
        await ResetDatabaseAsync();

        var createRequest = new CreateSchoolMstRequest
        {
            SchoolCode = "SCH001",
            SchoolName = "Integration Test School"
        };

        var createResponse =
            await _client.PostAsJsonAsync(
                "/api/Schools",
                createRequest);

        var createdSchool =
            await createResponse.Content
                .ReadFromJsonAsync<SchoolMstDto>();

        Assert.NotNull(createdSchool);

        // Act
        var response = await _client.GetAsync(
            $"/api/Schools/{createdSchool.Id}");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_WithEmptyCode_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();
        // Arrange
        var request = new CreateSchoolMstRequest
        {
            SchoolCode = "",
            SchoolName = "Integration Test School"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Schools",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_WithInvalidEmail_ShouldReturnBadRequest()
    {
        // Arrange
        await ResetDatabaseAsync();

        var request = new CreateSchoolMstRequest
        {
            SchoolCode = "SCH001",
            SchoolName = "Integration Test School",
            Email = "invalid-email"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Schools",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_WithInvalidCodeCharacters_ShouldReturnBadRequest()
    {
        // Arrange
        await ResetDatabaseAsync();

        var request = new CreateSchoolMstRequest
        {
            SchoolCode = "SCH@001",
            SchoolName = "Integration Test School"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Schools",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_WithEmptyCode_ShouldReturnValidationProblemDetails()
    {
        await ResetDatabaseAsync();
        // Arrange
        var request = new CreateSchoolMstRequest
        {
            SchoolCode = "",
            SchoolName = "Integration Test School"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Schools",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        var content =
            await response.Content.ReadAsStringAsync();

        // Debug: Output the actual response
        var output = $"Response: {content}";

        Assert.Contains(
        "Validation failed",
        content);

        Assert.Contains(
            "School code is required.",
            content);

        Assert.Contains(
            "Code",
            content);
    }

    [Fact]
    public async Task GetById_WhenSchoolDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            $"/api/Schools/{id}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_WithDuplicateCode_ShouldReturnConflict()
    {
        // Arrange
        await ResetDatabaseAsync();

        var firstRequest = new CreateSchoolMstRequest
        {
            SchoolCode = "SCH001",
            SchoolName = "First School"
        };

        var secondRequest = new CreateSchoolMstRequest
        {
            SchoolCode = "SCH001",
            SchoolName = "Second School"
        };

        // Act
        var firstResponse =
            await _client.PostAsJsonAsync(
                "/api/Schools",
                firstRequest);

        var secondResponse =
            await _client.PostAsJsonAsync(
                "/api/Schools",
                secondRequest);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            firstResponse.StatusCode);

        Assert.Equal(
            HttpStatusCode.Conflict,
            secondResponse.StatusCode);
    }

    [Fact]
    public async Task Update_WhenSchoolExists_ShouldReturnOk()
    {
        // Arrange
        await ResetDatabaseAsync();

        var createRequest = new CreateSchoolMstRequest
        {
            SchoolCode = "SCH001",
            SchoolName = "Original School"
        };

        var createResponse =
            await _client.PostAsJsonAsync(
                "/api/Schools",
                createRequest);

        var createdSchool =
            await createResponse.Content
                .ReadFromJsonAsync<SchoolMstDto>();

        Assert.NotNull(createdSchool);

        var updateRequest = new UpdateSchoolMstRequest
        {
            SchoolName = "Updated School",
            City = "Noida",
            State = "Uttar Pradesh"
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/Schools/{createdSchool.Id}",
            updateRequest);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Update_WhenSchoolDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        await ResetDatabaseAsync();

        var updateRequest = new UpdateSchoolMstRequest
        {
            SchoolName = "Updated School"
        };

        var id = Guid.NewGuid();

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/Schools/{id}",
            updateRequest);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Deactivate_WhenSchoolExists_ShouldReturnOk()
    {
        // Arrange
        await ResetDatabaseAsync();

        var createRequest = new CreateSchoolMstRequest
        {
            SchoolCode = "SCH001",
            SchoolName = "Test School"
        };

        var createResponse =
            await _client.PostAsJsonAsync(
                "/api/Schools",
                createRequest);

        var school =
            await createResponse.Content
                .ReadFromJsonAsync<SchoolMstDto>();

        Assert.NotNull(school);

        // Act
        var response = await _client.PatchAsync(
            $"/api/Schools/{school.Id}/deactivate",
            null);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Activate_WhenSchoolExists_ShouldReturnOk()
    {
        // Arrange
        await ResetDatabaseAsync();

        var createRequest = new CreateSchoolMstRequest
        {
            SchoolCode = "SCH001",
            SchoolName = "Test School"
        };

        var createResponse =
            await _client.PostAsJsonAsync(
                "/api/Schools",
                createRequest);

        var school =
            await createResponse.Content
                .ReadFromJsonAsync<SchoolMstDto>();

        Assert.NotNull(school);

        await _client.PatchAsync(
            $"/api/Schools/{school.Id}/deactivate",
            null);

        // Act
        var response = await _client.PatchAsync(
            $"/api/Schools/{school.Id}/activate",
            null);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Deactivate_WhenSchoolDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        await ResetDatabaseAsync();

        var id = Guid.NewGuid();

        // Act
        var response = await _client.PatchAsync(
            $"/api/Schools/{id}/deactivate",
            null);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Activate_WhenSchoolDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        await ResetDatabaseAsync();

        var id = Guid.NewGuid();

        // Act
        var response = await _client.PatchAsync(
            $"/api/Schools/{id}/activate",
            null);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task GetById_WithInvalidGuid_ShouldReturnNotFound()
    {
        // Arrange
        await ResetDatabaseAsync();

        // Act
        var response = await _client.GetAsync(
            "/api/Schools/not-a-guid");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    //[Fact]
    //public async Task Create_WithEmptyCode_ShouldReturnValidationProblemDetails_()
    //{
    //    // Arrange
    //    await ResetDatabaseAsync();

    //    var request = new CreateSchoolMstRequest
    //    {
    //        SchoolCode = "",
    //        SchoolName = "Integration Test School"
    //    };

    //    // Act
    //    var response = await _client.PostAsJsonAsync(
    //        "/api/Schools",
    //        request);

    //    // Assert
    //    Assert.Equal(
    //        HttpStatusCode.BadRequest,
    //        response.StatusCode);

    //    var json =
    //        await response.Content
    //            .ReadFromJsonAsync<JsonElement>();

    //    Assert.Equal(
    //        "Validation failed",
    //        json.GetProperty("title").GetString());

    //    Assert.Equal(
    //        400,
    //        json.GetProperty("status").GetInt32());

    //    Assert.True(
    //        json.TryGetProperty(
    //            "errors",
    //            out var errors));

    //    Assert.True(
    //        errors.TryGetProperty(
    //            "Code",
    //            out var codeErrors));

    //    Assert.Contains(
    //        "School code is required.",
    //        codeErrors
    //            .EnumerateArray()
    //            .Select(x => x.GetString()));
    //}
}
