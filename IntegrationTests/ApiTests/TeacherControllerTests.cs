using IntegrationTests.Infrastructure;
using IntegrationTests.TestData;
using SMS.Application.DTOs;
using SMS.Domain.Enums;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.ApiTests;

[Collection("Integration Tests")]
public sealed class TeacherControllerTests : IntegrationTestBase
{
    public TeacherControllerTests(IntegrationTestFixture fixture) : base(fixture.Factory)
    {
    }
    private const string Endpoint = "/api/teachers";

    [Fact]
    public async Task Create_ShouldReturnCreated()
    {
        await ResetDatabaseAsync();

        var schoolId = await CreateSchoolAsync();
        var request =
            TeacherTestData.CreateRequest(
                schoolId: schoolId,
                teacherCode: "TCH001",
                employeeCode: "EMP001");

        var response =
            await Client.PostAsJsonAsync(Endpoint, request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result =
            await response.Content.ReadFromJsonAsync<TeacherMstResponse>();

        Assert.NotNull(result);
        Assert.True(result!.TeacherId > 0);
        Assert.Equal(schoolId, result.SchoolId);
        Assert.Equal("TCH001", result.TeacherCode);
        Assert.Equal("EMP001", result.EmployeeCode);
        Assert.Equal("Rahul", result.FirstName);
        Assert.Equal("Sharma", result.LastName);
        Assert.True(result.IsActive);
    }

     [Fact]
    public async Task Create_WithInvalidRequest_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();

        var request = new CreateTeacherMstRequest
        {
            SchoolId = 0,
            TeacherCode = "",
            EmployeeCode = "",
            FirstName = "",
            LastName = "",
            Gender = (Gender)999,
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            Email = "invalid-email",
            MobileNumber = "123",
            DateOfJoining = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
        };

        var response =
            await Client.PostAsJsonAsync(Endpoint, request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithNonExistingSchool_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();
        var request = TeacherTestData.CreateRequest(schoolId: 999999);

        var response =
            await Client.PostAsJsonAsync(
                Endpoint, request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithDuplicateTeacherCode_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();
        var schoolId = await CreateSchoolAsync();

        var firstRequest =
            TeacherTestData.CreateRequest(
                schoolId,
                teacherCode: "TCH001",
                employeeCode: "EMP001");

        var firstResponse =
            await Client.PostAsJsonAsync(
                Endpoint, firstRequest);

        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        var secondRequest =
            TeacherTestData.CreateRequest(
                schoolId,
                teacherCode: "TCH001",
                employeeCode: "EMP002");

        var secondResponse =
            await Client.PostAsJsonAsync(
                Endpoint, secondRequest);

        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task Create_WithDuplicateEmployeeCode_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();
        var schoolId = await CreateSchoolAsync();

        var firstRequest =
            TeacherTestData.CreateRequest(
                schoolId,
                teacherCode: "TCH001",
                employeeCode: "EMP001");

        var firstResponse =
            await Client.PostAsJsonAsync(
                Endpoint, firstRequest);

        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        var secondRequest =
            TeacherTestData.CreateRequest(
                schoolId,
                teacherCode: "TCH002",
                employeeCode: "EMP001");

        var secondResponse =
            await Client.PostAsJsonAsync(
                Endpoint, secondRequest);

        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task Create_SameCodesInDifferentSchools_ShouldReturnCreated()
    {
        await ResetDatabaseAsync();
        var school1Id = await CreateSchoolAsync("SCH001");
        var school2Id = await CreateSchoolAsync("SCH002");

        var request1 =
            TeacherTestData.CreateRequest(
                school1Id,
                "TCH001",
                "EMP001");

        var response1 =
            await Client.PostAsJsonAsync(
                Endpoint, request1);

        Assert.Equal(HttpStatusCode.Created, response1.StatusCode);

        var request2 =
            TeacherTestData.CreateRequest(
                school2Id,
                "TCH001",
                "EMP001");

        var response2 =
            await Client.PostAsJsonAsync(
                Endpoint, request2);

        Assert.Equal(HttpStatusCode.Created, response2.StatusCode);
    }

    [Fact]
    public async Task GetById_WhenTeacherExists_ShouldReturnOk()
    {
        await ResetDatabaseAsync();
        var schoolId =
            await CreateSchoolAsync();

        var createResponse =
            await Client.PostAsJsonAsync(
                Endpoint, TeacherTestData.CreateRequest(schoolId));

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created =
            await createResponse.Content.ReadFromJsonAsync<TeacherMstResponse>();

        Assert.NotNull(created);

        var response =
            await Client.GetAsync(
                $"/api/teachers/{created!.TeacherId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<TeacherMstResponse>();

        Assert.NotNull(result);
        Assert.Equal(created.TeacherId, result!.TeacherId);
    }

    [Fact]
    public async Task GetById_WhenTeacherDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var response =
            await Client.GetAsync(
                "/api/teachers/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_WhenTeacherExists_ShouldReturnOk()
    {
        await ResetDatabaseAsync();
        var schoolId = await CreateSchoolAsync();

        var createResponse =
            await Client.PostAsJsonAsync(
                Endpoint, TeacherTestData.CreateRequest(schoolId));

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created =
            await createResponse.Content.ReadFromJsonAsync<TeacherMstResponse>();

        Assert.NotNull(created);

        var updateRequest = TeacherTestData.UpdateRequest();

        var updateResponse =
            await Client.PutAsJsonAsync(
                $"/api/teachers/{created!.TeacherId}", updateRequest);

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var result =
            await updateResponse.Content
                .ReadFromJsonAsync<TeacherMstResponse>();

        Assert.NotNull(result);
        Assert.Equal("Amit", result!.FirstName);
        Assert.Equal("Senior Teacher", result.Designation);
        // Identity fields must remain unchanged.
        Assert.Equal("TCH001", result.TeacherCode);
        Assert.Equal("EMP001", result.EmployeeCode);
    }

    [Fact]
    public async Task Update_WhenTeacherDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();
        var request = TeacherTestData.UpdateRequest();

        var response =
            await Client.PutAsJsonAsync(
                "/api/teachers/999999", request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Deactivate_ShouldReturnNoContent()
    {
        await ResetDatabaseAsync();
        var schoolId =
            await CreateSchoolAsync();

        var createResponse =
            await Client.PostAsJsonAsync(
                Endpoint, TeacherTestData.CreateRequest(schoolId));

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<TeacherMstResponse>();

        Assert.NotNull(created);

        var response =
            await Client.PatchAsync(
                $"/api/teachers/{created!.TeacherId}/deactivate", null);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse =
            await Client.GetAsync(
                $"/api/teachers/{created.TeacherId}");

        var teacher =
            await getResponse.Content
                .ReadFromJsonAsync<TeacherMstResponse>();

        Assert.NotNull(teacher);
        Assert.False(teacher!.IsActive);
    }

    [Fact]
    public async Task Activate_ShouldReturnNoContent()
    {
        await ResetDatabaseAsync();
        var schoolId = await CreateSchoolAsync();

        var createResponse =
            await Client.PostAsJsonAsync(Endpoint, TeacherTestData.CreateRequest(schoolId));

        var created =
            await createResponse.Content.ReadFromJsonAsync<TeacherMstResponse>();
        Assert.NotNull(created);

        await Client.PatchAsync($"/api/teachers/{created!.TeacherId}/deactivate", null);

        var response =
            await Client.PatchAsync($"/api/teachers/{created.TeacherId}/activate", null);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse =
            await Client.GetAsync($"/api/teachers/{created.TeacherId}");

        var teacher =
            await getResponse.Content.ReadFromJsonAsync<TeacherMstResponse>();

        Assert.NotNull(teacher);
        Assert.True(teacher!.IsActive);
    }

    [Fact]
    public async Task GetAll_ShouldReturnTeachers()
    {
        await ResetDatabaseAsync();
        var schoolId = await CreateSchoolAsync();
        var response1 =
            await Client.PostAsJsonAsync(
                Endpoint,
                TeacherTestData.CreateRequest(
                    schoolId,
                    "TCH001",
                    "EMP001"));

        var response2 =
            await Client.PostAsJsonAsync(
                Endpoint,
                TeacherTestData.CreateRequest(
                    schoolId,
                    "TCH002",
                    "EMP002"));

        Assert.Equal(HttpStatusCode.Created, response1.StatusCode);
        Assert.Equal(HttpStatusCode.Created, response2.StatusCode);
        var response =
            await Client.GetAsync(Endpoint);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<List<TeacherMstResponse>>();

        Assert.NotNull(result);
        Assert.Equal(2, result!.Count);
    }


}
