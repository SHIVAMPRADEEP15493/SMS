using IntegrationTests.Infrastructure;
using IntegrationTests.TestData;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using IntegrationTests.Infrastructure;
using SMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using SMS.Application.DTOs.Response;

namespace IntegrationTests.ApiTests;

[Collection("Integration Tests")]

public sealed class StudentEnrollmentControllerTests : IntegrationTestBase
{
    private const string Endpoint = "/api/StudentEnrollments";

    public StudentEnrollmentControllerTests(
        IntegrationTestFixture fixture)
        : base(fixture.Factory)
    {
    }

    [Fact]
    public async Task Create_ShouldReturnCreated()
    {
        await ResetDatabaseAsync();

        var request = StudentEnrollmentTestData.CreateRequest();

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<StudentEnrollmentResponse>();

        Assert.NotNull(result);
        Assert.True(result.StudentEnrollmentId > 0);
        Assert.Equal(request.StudentId, result.StudentId);
        Assert.Equal(request.SchoolId, result.SchoolId);
        Assert.Equal(request.SessionId, result.SessionId);
        Assert.Equal(request.ClassId, result.ClassId);
        Assert.Equal(request.SectionId, result.SectionId);
        Assert.Equal(request.AdmissionDate, result.AdmissionDate);
        Assert.Equal(request.RollNumber, result.RollNumber);
        Assert.Equal("Active", result.Status);
    }

    [Fact]
    public async Task Create_DuplicateStudentAndSession_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var request = StudentEnrollmentTestData.CreateRequest();

        var firstResponse = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        var duplicateResponse = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(
            HttpStatusCode.Conflict,
            duplicateResponse.StatusCode);
    }

    [Fact]
    public async Task Create_SameStudentDifferentSession_ShouldReturnCreated()
    {
        await ResetDatabaseAsync();

        var firstRequest = StudentEnrollmentTestData.CreateRequest(
            studentId: 1001,
            sessionId: 2);

        var secondRequest = StudentEnrollmentTestData.CreateRequest(
            studentId: 1001,
            sessionId: 3);

        var firstResponse = await Client.PostAsJsonAsync(
            Endpoint,
            firstRequest);

        var secondResponse = await Client.PostAsJsonAsync(
            Endpoint,
            secondRequest);

        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Created, secondResponse.StatusCode);
    }

    [Fact]
    public async Task Create_InvalidRequest_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();

        var request = StudentEnrollmentTestData.CreateRequest(
            studentId: 0,
            sessionId: -2,
            rollNumber: " ");

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task GetById_ExistingEnrollment_ShouldReturnOk()
    {
        await ResetDatabaseAsync();

        var enrollmentId = await CreateEnrollmentAsync();

        var response = await Client.GetAsync(
            $"{Endpoint}/{enrollmentId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<StudentEnrollmentResponse>();

        Assert.NotNull(result);
        Assert.Equal(enrollmentId, result.StudentEnrollmentId);
    }

    [Fact]
    public async Task GetById_NonExistingEnrollment_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync($"{Endpoint}/999999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        await ResetDatabaseAsync();

        await CreateEnrollmentAsync(studentId: 1001, sessionId: 2);
        await CreateEnrollmentAsync(studentId: 1002, sessionId: 2);

        var response = await Client.GetAsync(Endpoint);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<
                List<StudentEnrollmentResponse>>();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Update_ExistingEnrollment_ShouldUpdatePlacement()
    {
        await ResetDatabaseAsync();

        var enrollmentId = await CreateEnrollmentAsync();

        var request = StudentEnrollmentTestData.UpdateRequest(
            classId: 6,
            sectionId: 4,
            rollNumber: "12");

        var response = await Client.PutAsJsonAsync(
            $"{Endpoint}/{enrollmentId}",
            request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<StudentEnrollmentResponse>();

        Assert.NotNull(result);
        Assert.Equal(enrollmentId, result.StudentEnrollmentId);
        Assert.Equal(1001, result.StudentId);
        Assert.Equal(2, result.SessionId);
        Assert.Equal(6, result.ClassId);
        Assert.Equal(4, result.SectionId);
        Assert.Equal("12", result.RollNumber);
    }

    [Fact]
    public async Task Update_NonExistingEnrollment_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var request = StudentEnrollmentTestData.UpdateRequest();

        var response = await Client.PutAsJsonAsync(
            $"{Endpoint}/999999",
            request);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Theory]
    [InlineData("Completed")]
    [InlineData("Transferred")]
    [InlineData("Withdrawn")]
    public async Task UpdateStatus_TerminalStatus_ShouldReturnOk(
        string status)
    {
        await ResetDatabaseAsync();

        var enrollmentId = await CreateEnrollmentAsync();

        var request = StudentEnrollmentTestData.StatusRequest(status);

        var response = await Client.PatchAsJsonAsync(
            $"{Endpoint}/{enrollmentId}/status",
            request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<StudentEnrollmentResponse>();

        Assert.NotNull(result);
        Assert.Equal(status, result.Status);
    }

    [Fact]
    public async Task UpdateStatus_TerminalEnrollmentToDifferentStatus_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var enrollmentId = await CreateEnrollmentAsync();

        var completeRequest =
            StudentEnrollmentTestData.StatusRequest("Completed");

        var completeResponse = await Client.PatchAsJsonAsync(
            $"{Endpoint}/{enrollmentId}/status",
            completeRequest);

        Assert.Equal(HttpStatusCode.OK, completeResponse.StatusCode);

        var reopenRequest =
            StudentEnrollmentTestData.StatusRequest("Active");

        var reopenResponse = await Client.PatchAsJsonAsync(
            $"{Endpoint}/{enrollmentId}/status",
            reopenRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            reopenResponse.StatusCode);
    }

    [Fact]
    public async Task UpdateStatus_InvalidStatus_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();

        var enrollmentId = await CreateEnrollmentAsync();

        var request =
            StudentEnrollmentTestData.StatusRequest("InvalidStatus");

        var response = await Client.PatchAsJsonAsync(
            $"{Endpoint}/{enrollmentId}/status",
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Update_TerminalEnrollment_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var enrollmentId = await CreateEnrollmentAsync();

        var statusRequest =
            StudentEnrollmentTestData.StatusRequest("Transferred");

        var statusResponse = await Client.PatchAsJsonAsync(
            $"{Endpoint}/{enrollmentId}/status",
            statusRequest);

        Assert.Equal(HttpStatusCode.OK, statusResponse.StatusCode);

        var updateRequest = StudentEnrollmentTestData.UpdateRequest();

        var updateResponse = await Client.PutAsJsonAsync(
            $"{Endpoint}/{enrollmentId}",
            updateRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            updateResponse.StatusCode);
    }

    private async Task<long> CreateEnrollmentAsync(
        long studentId = 1001,
        long sessionId = 2)
    {
        var request = StudentEnrollmentTestData.CreateRequest(
            studentId: studentId,
            sessionId: sessionId);

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<StudentEnrollmentResponse>();

        Assert.NotNull(result);

        return result.StudentEnrollmentId;
    }
}
