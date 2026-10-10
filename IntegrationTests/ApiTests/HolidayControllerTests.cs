using IntegrationTests.Infrastructure;
using IntegrationTests.TestData;
using SMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace IntegrationTests.ApiTests;

[Collection("Integration Tests")]
public class HolidayControllerTests : IntegrationTestBase
{
    public HolidayControllerTests(IntegrationTestFixture fixture)
       : base(fixture.Factory)
    {
    }
    private const string Endpoint = "/api/Holidays";


    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync(Endpoint);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WhenHolidayDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync($"{Endpoint}/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenDescriptionIsEmpty_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();

        var request = new CreateHolidayDtlRequest
        {
            SchoolId = 1,
            SessionId = 1,
            HolidayDate = new DateOnly(2026, 10, 20),
            HolidayDescription = string.Empty
        };

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenSchoolDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var request = new CreateHolidayDtlRequest
        {
            SchoolId = 999999,
            SessionId = 1,
            HolidayDate = new DateOnly(2026, 10, 20),
            HolidayDescription = "Test Holiday"
        };

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_WhenHolidayDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var request = new UpdateHolidayDtlRequest
        {
            SchoolId = 1,
            SessionId = 1,
            HolidayDate = new DateOnly(2026, 10, 21),
            HolidayDescription = "Updated Holiday"
        };

        var response = await Client.PutAsJsonAsync(
            $"{Endpoint}/999999",
            request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenHolidayDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var response = await Client.DeleteAsync($"{Endpoint}/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidRequest_ShouldReturnCreated()
    {
        await ResetDatabaseAsync();

        // Use your existing integration-test helpers to create
        // a valid school and a session covering October 2026.
        var createRequest = new CreateSchoolMstRequest
        {
            SchoolCode = "SCH001",
            SchoolName = "Integration Test School"
        };

        var createResponse = await Client.PostAsJsonAsync("/api/Schools", createRequest);
        var createdSchool = await createResponse.Content.ReadFromJsonAsync<SchoolMstDto>();

        //var sessionRequest = new CreateSessionMstRequest
        //{
        //    SessionCode = "2026",
        //    SessionName = "Academic Session 2026-27",
        //    StartDate = new DateOnly(2026, 4, 1),
        //    EndDate = new DateOnly(2027, 3, 31),
        //    IsCurrent = false
        //};
        var sessionRequest = SessionTestData.CreateRequest(code: "2026-27", isCurrent: true);
        var sessionResponse = await Client.PostAsJsonAsync("/api/sessions", sessionRequest);
        //var sessionResponse = await Client.PostAsJsonAsync("/api/sessions", sessionRequest);
        var createdSession = await sessionResponse.Content.ReadFromJsonAsync<SessionMstResponse>();

        var holidayRequest = new CreateHolidayDtlRequest
        {
            SchoolId = createdSchool.Id,
            SessionId = createdSession.SessionId,
            HolidayDate = new DateOnly(2026, 10, 20),
            HolidayDescription = "Diwali"
        };

        var holidayResponse = await Client.PostAsJsonAsync(
            Endpoint,
            holidayRequest);

        Assert.Equal(HttpStatusCode.Created, holidayResponse.StatusCode);

        var result = await holidayResponse.Content
            .ReadFromJsonAsync<HolidayDtlResponse>();

        Assert.NotNull(result);     
        Assert.True(result.HolidayId > 0);
        Assert.Equal(createdSchool.Id, result.SchoolId);
        Assert.Equal(createdSession.SessionId, result.SessionId);
        Assert.Equal(new DateOnly(2026, 10, 20), result.HolidayDate);
        Assert.Equal("Diwali", result.HolidayDescription);
    }
}
