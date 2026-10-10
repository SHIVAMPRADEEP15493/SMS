using IntegrationTests.Infrastructure;
using SMS.Application.DTOs;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Response;
using SMS.Application.DTOs.Update;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace IntegrationTests.ApiTests;

[Collection("Integration Tests")]
public sealed class StudentControllerTests : IntegrationTestBase
{
    public StudentControllerTests(IntegrationTestFixture fixture) : base(fixture.Factory)
    {
    }
    private const string Endpoint = "/api/students";

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenRequestIsValid()
    {
        await ResetDatabaseAsync();

        var request = CreateValidStudentRequest();

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var student = await response.Content
            .ReadFromJsonAsync<StudentMstResponse>();

        Assert.NotNull(student);
        Assert.True(student.StudentId > 0);
        Assert.Equal(request.StudentCode, student.StudentCode);
        Assert.Equal(request.AdmissionNumber, student.AdmissionNumber);
        Assert.Equal(request.FirstName, student.FirstName);
        Assert.Equal("Active", student.CurrentStatus);
        Assert.True(student.IsActive);
    }

    [Fact]
    public async Task Create_ShouldReturnConflict_WhenStudentCodeAlreadyExists()
    {
        await ResetDatabaseAsync();

        var request = CreateValidStudentRequest();

        var firstResponse = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        var duplicateRequest = CreateValidStudentRequest();
        duplicateRequest.AdmissionNumber = "ADM-1002";

        var duplicateResponse = await Client.PostAsJsonAsync(
            Endpoint,
            duplicateRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            duplicateResponse.StatusCode);

        var body = await duplicateResponse.Content.ReadAsStringAsync();

        Assert.Contains("Conflict", body);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        await ResetDatabaseAsync();

        var request = CreateValidStudentRequest();
        request.StudentCode = "";
        request.AdmissionNumber = "";
        request.FirstName = "";

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();

        Assert.Contains("Validation failed", body);
    }

    [Fact]
    public async Task GetById_ShouldReturnStudent_WhenStudentExists()
    {
        await ResetDatabaseAsync();

        var created = await CreateStudentAsync();

        var response = await Client.GetAsync(
            $"/api/students/{created.StudentId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var student = await response.Content
            .ReadFromJsonAsync<StudentMstResponse>();

        Assert.NotNull(student);
        Assert.Equal(created.StudentId, student.StudentId);
        Assert.Equal(created.StudentCode, student.StudentCode);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenStudentDoesNotExist()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync(
            "/api/students/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllStudents()
    {
        await ResetDatabaseAsync();

        await CreateStudentAsync("STU-1001", "ADM-1001");
        await CreateStudentAsync("STU-1002", "ADM-1002");

        var response = await Client.GetAsync(Endpoint);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var students = await response.Content
            .ReadFromJsonAsync<List<StudentMstResponse>>();

        Assert.NotNull(students);
        Assert.Equal(2, students.Count);
        Assert.Contains(students, s => s.StudentCode == "STU-1001");
        Assert.Contains(students, s => s.StudentCode == "STU-1002");
    }

    [Fact]
    public async Task Update_ShouldReturnUpdatedStudent_WhenRequestIsValid()
    {
        await ResetDatabaseAsync();

        var created = await CreateStudentAsync();

        var request = new UpdateStudentMstRequest
        {
            FirstName = "Updated",
            MiddleName = "Integration",
            LastName = "Student",
            DateOfBirth = new DateOnly(2012, 5, 15),
            Gender = "Female",
            MobileNumber = "9876543210",
            Email = "updated.student@example.com",
            Address = "Updated address",
            FatherName = "Father Name",
            MotherName = "Mother Name",
            GuardianName = null,
            UpdatedBy = 1
        };

        var response = await Client.PutAsJsonAsync(
            $"/api/students/{created.StudentId}",
            request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updated = await response.Content
            .ReadFromJsonAsync<StudentMstResponse>();

        Assert.NotNull(updated);
        Assert.Equal("Updated", updated.FirstName);
        Assert.Equal("Integration", updated.MiddleName);
        Assert.Equal("updated.student@example.com", updated.Email);

        // Admission identity fields must remain unchanged.
        Assert.Equal(created.StudentCode, updated.StudentCode);
        Assert.Equal(created.AdmissionNumber, updated.AdmissionNumber);
        Assert.Equal(created.AdmissionDate, updated.AdmissionDate);
        Assert.Equal(
            created.AdmittedSessionId,
            updated.AdmittedSessionId);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenStudentDoesNotExist()
    {
        await ResetDatabaseAsync();

        var request = CreateValidUpdateRequest();

        var response = await Client.PutAsJsonAsync(
            "/api/students/999999",
            request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("Completed")]
    [InlineData("Transferred")]
    [InlineData("Withdrawn")]
    public async Task UpdateStatus_ShouldUpdateStatusAndDeactivateStudent(
        string status)
    {
        await ResetDatabaseAsync();

        var created = await CreateStudentAsync();

        var response = await Client.PatchAsJsonAsync(
            $"/api/students/{created.StudentId}/status",
            new UpdateStudentStatusRequest
            {
                Status = status,
                UpdatedBy = 1
            });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify the persisted state through the API.
        var getResponse = await Client.GetAsync(
            $"/api/students/{created.StudentId}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var student = await getResponse.Content
            .ReadFromJsonAsync<StudentMstResponse>();

        Assert.NotNull(student);
        Assert.Equal(status, student.CurrentStatus);
        Assert.False(student.IsActive);
    }

    [Fact]
    public async Task UpdateStatus_ShouldReturnConflict_WhenChangingTerminalStatus()
    {
        await ResetDatabaseAsync();

        var created = await CreateStudentAsync();

        var completeResponse = await Client.PatchAsJsonAsync(
            $"/api/students/{created.StudentId}/status",
            new UpdateStudentStatusRequest
            {
                Status = "Completed",
                UpdatedBy = 1
            });

        Assert.Equal(
            HttpStatusCode.NoContent,
            completeResponse.StatusCode);

        var changeResponse = await Client.PatchAsJsonAsync(
            $"/api/students/{created.StudentId}/status",
            new UpdateStudentStatusRequest
            {
                Status = "Transferred",
                UpdatedBy = 1
            });

        Assert.Equal(
            HttpStatusCode.Conflict,
            changeResponse.StatusCode);
    }

    //[Fact]
    //public async Task UpdateStatus_ShouldReturnBadRequest_WhenStatusIsInvalid()
    //{
    //    await ResetDatabaseAsync();

    //    var created = await CreateStudentAsync();

    //    var response = await Client.PatchAsJsonAsync(
    //        $"/api/students/{created.StudentId}/status",
    //        new UpdateStudentStatusRequest
    //        {
    //            Status = "Failed",
    //            UpdatedBy = 1
    //        });

    //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    //}

    [Fact]
    public async Task UpdateStatus_ShouldReturnNotFound_WhenStudentDoesNotExist()
    {
        await ResetDatabaseAsync();

        var response = await Client.PatchAsJsonAsync(
            $"{Endpoint}/999999/status",
            new UpdateStudentStatusRequest
            {
                Status = "Completed",
                UpdatedBy = 1
            });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<StudentMstResponse> CreateStudentAsync(
        string studentCode = "STU-1001",
        string admissionNumber = "ADM-1001")
    {
        var request = CreateValidStudentRequest();

        request.StudentCode = studentCode;
        request.AdmissionNumber = admissionNumber;

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var student = await response.Content
            .ReadFromJsonAsync<StudentMstResponse>();

        Assert.NotNull(student);

        return student;
    }

    private static CreateStudentMstRequest CreateValidStudentRequest()
    {
        return new CreateStudentMstRequest
        {
            StudentCode = "STU-1001",
            AdmissionNumber = "ADM-1001",
            AdmissionDate = new DateOnly(2026, 4, 10),
            AdmittedSessionId = 1,
            FirstName = "Test",
            MiddleName = null,
            LastName = "Student",
            DateOfBirth = new DateOnly(2012, 5, 15),
            Gender = "Male",
            MobileNumber = "9876543210",
            Email = "test.student@example.com",
            Address = "Test address",
            FatherName = "Father Name",
            MotherName = "Mother Name",
            GuardianName = null,
            CreatedBy = 1
        };
    }

    private static UpdateStudentMstRequest CreateValidUpdateRequest()
    {
        return new UpdateStudentMstRequest
        {
            FirstName = "Updated",
            MiddleName = null,
            LastName = "Student",
            DateOfBirth = new DateOnly(2012, 5, 15),
            Gender = "Female",
            MobileNumber = "9876543210",
            Email = "updated.student@example.com",
            Address = "Updated address",
            FatherName = "Father Name",
            MotherName = "Mother Name",
            GuardianName = null,
            UpdatedBy = 1
        };
    }
}
