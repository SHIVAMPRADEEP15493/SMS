using IntegrationTests.Infrastructure;
using IntegrationTests.TestData;
using SMS.Application.DTOs;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System;
using System.Text;
using System.Collections.Generic;

namespace IntegrationTests.ApiTests;

[Collection("Integration Tests")]
public class SchoolsControllerTests : IntegrationTestBase
{
    public SchoolsControllerTests(IntegrationTestFixture fixture) : base(fixture.Factory)
    {
    }
    private const string Endpoint = "/api/Schools";

    private async Task ResetDatabaseAsync()
    {
        await Factory.ResetDatabaseAsync();
    }
    [Fact]
    public async Task Create_WithValidRequest_ShouldReturnCreated()
    {
        await ResetDatabaseAsync();
        // Arrange
        var request = SchoolTestData.CreateRequest();

        // Act
        var response = await Client.PostAsJsonAsync(Endpoint, request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Database_ShouldStartClean()
    {
        await ResetDatabaseAsync();
        var response = await Client.GetAsync(Endpoint);
        response.EnsureSuccessStatusCode();

        var schools = await response.Content.ReadFromJsonAsync<List<SchoolMstDto>>();
        Assert.NotNull(schools);
        Assert.Empty(schools);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        await ResetDatabaseAsync();
        // Act
        var response = await Client.GetAsync(Endpoint);

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

        var createResponse = await Client.PostAsJsonAsync(Endpoint, createRequest);
        var createdSchool = await createResponse.Content.ReadFromJsonAsync<SchoolMstDto>();

        Assert.NotNull(createdSchool);

        // Act
        var response = await Client.GetAsync($"{Endpoint}/{createdSchool.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithEmptyCode_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();
        // Arrange
        var request = SchoolTestData.CreateRequestWithEmptyCode();

        // Act
        var response = await Client.PostAsJsonAsync(Endpoint, request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithInvalidEmail_ShouldReturnBadRequest()
    {
        // Arrange
        await ResetDatabaseAsync();
        var request = SchoolTestData.CreateRequestWithInvalidEmail();

        // Act
        var response = await Client.PostAsJsonAsync(Endpoint, request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithInvalidCodeCharacters_ShouldReturnBadRequest()
    {
        // Arrange
        await ResetDatabaseAsync();
        var request = SchoolTestData.CreateRequestWithInvalidCode();

        // Act
        var response = await Client.PostAsJsonAsync(Endpoint, request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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
        var response = await Client.PostAsJsonAsync(Endpoint, request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();

        // Debug: Output the actual response
        var output = $"Response: {content}";
        Assert.Contains("Validation failed", content);
        Assert.Contains("School code is required.", content);
        Assert.Contains("Code", content);
    }

    [Fact]
    public async Task GetById_WhenSchoolDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync( $"{Endpoint}/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithDuplicateCode_ShouldReturnConflict()
    {
        // Arrange
        await ResetDatabaseAsync();
        var code = $"DUP-{Guid.NewGuid():N}";
        var firstRequest = SchoolTestData.CreateRequest(code: code, name: "First School");
        var secondRequest = SchoolTestData.CreateRequest(code: code, name: "Second School");

        // Act
        var firstResponse = await Client.PostAsJsonAsync(Endpoint, firstRequest);
        var secondResponse = await Client.PostAsJsonAsync(Endpoint, secondRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task Update_WhenSchoolExists_ShouldReturnOk()
    {
        // Arrange
        await ResetDatabaseAsync();
        var createRequest = SchoolTestData.CreateRequest();
        var createResponse = await Client.PostAsJsonAsync(Endpoint, createRequest);
        var createdSchool =await createResponse.Content.ReadFromJsonAsync<SchoolMstDto>();
        Assert.NotNull(createdSchool);
        var updateRequest = new UpdateSchoolMstRequest
        {
            SchoolName = "Updated School",
            City = "Noida",
            State = "Uttar Pradesh"
        };

        // Act
        var response = await Client.PutAsJsonAsync($"{Endpoint}/{createdSchool.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
        var response = await Client.PutAsJsonAsync($"{Endpoint}/{id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
        var createResponse = await Client.PostAsJsonAsync(Endpoint, createRequest);
        var school = await createResponse.Content.ReadFromJsonAsync<SchoolMstDto>();
        Assert.NotNull(school);

        // Act
        var response = await Client.PatchAsync($"{Endpoint}/{school.Id}/deactivate", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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

        var createResponse = await Client.PostAsJsonAsync(Endpoint, createRequest);
        var school = await createResponse.Content.ReadFromJsonAsync<SchoolMstDto>();
        Assert.NotNull(school);
        await Client.PatchAsync($"{Endpoint}/{school.Id}/deactivate", null);

        // Act
        var response = await Client.PatchAsync($"{Endpoint}/{school.Id}/activate", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Deactivate_WhenSchoolDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        await ResetDatabaseAsync();
        var id = Guid.NewGuid();

        // Act
        var response = await Client.PatchAsync($"{Endpoint}/{id}/deactivate", null);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Activate_WhenSchoolDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        await ResetDatabaseAsync();
        var id = Guid.NewGuid();

        // Act
        var response = await Client.PatchAsync($"{Endpoint}/{id}/activate", null);
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WithInvalidGuid_ShouldReturnNotFound()
    {
        // Arrange
        await ResetDatabaseAsync();
        // Act
        var response = await Client.GetAsync($"{Endpoint}/not-a-guid");
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    /*
    [Fact]
    public async Task Create_WithEmptyCode_ShouldReturnValidationProblemDetails_()
    {
        // Arrange
        await ResetDatabaseAsync();

        var request = new CreateSchoolMstRequest
        {
            SchoolCode = "",
            SchoolName = "Integration Test School"
        };

        // Act
        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        var json =
            await response.Content
                .ReadFromJsonAsync<JsonElement>();

        Assert.Equal(
            "Validation failed",
            json.GetProperty("title").GetString());

        Assert.Equal(
            400,
            json.GetProperty("status").GetInt32());

        Assert.True(
            json.TryGetProperty(
                "errors",
                out var errors));

        Assert.True(
            errors.TryGetProperty(
                "Code",
                out var codeErrors));

        Assert.Contains(
            "School code is required.",
            codeErrors
                .EnumerateArray()
                .Select(x => x.GetString()));
    }
    */

    [Fact]
    public async Task Create_WithDuplicateCode_ShouldReturnConflictProblemDetails()
    {
        // Arrange
        await ResetDatabaseAsync();
        var request = new CreateSchoolMstRequest
        {
            SchoolCode = "SCH001",
            SchoolName = "First School"
        };

        await Client.PostAsJsonAsync(Endpoint, request);

        // Act
        var response = await Client.PostAsJsonAsync(Endpoint, request);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal("Conflict", json.GetProperty("title").GetString());

        Assert.Equal(409, json.GetProperty("status").GetInt32());
    }

    [Fact]
    public async Task Create_WithValidRequest_ShouldReturnCreatedResource()
    {
        // Arrange
        await ResetDatabaseAsync();
        var request = SchoolTestData.CreateRequest();

        // Act
        var response = await Client.PostAsJsonAsync(Endpoint, request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var school = await response.Content.ReadFromJsonAsync<SchoolMstDto>();
        Assert.NotNull(school);
        Assert.NotEqual(0L, school.Id);
        Assert.Equal(request.SchoolCode, school.SchoolCode);
        Assert.Equal(request.SchoolName, school.SchoolName);
        Assert.True(school.IsActive);
    }
    [Fact]
    public async Task GetById_WhenSchoolDoesNotExist_ShouldReturnNotFoundProblemDetails()
    {
        // Arrange
        await ResetDatabaseAsync();
        var id = 1L; // Replace with actual school ID

        // Act
        var response = await Client.GetAsync($"{Endpoint}/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Resource not found", json.GetProperty("title").GetString());
        Assert.Equal(404, json.GetProperty("status").GetInt32());
        Assert.True(json.TryGetProperty("traceId", out var traceId));
        Assert.False(string.IsNullOrWhiteSpace(traceId.GetString()));
    }
}
