using IntegrationTests.Infrastructure;
using IntegrationTests.TestData;
using SMS.Application.DTOs;
using System.Net;
using System.Net.Http.Json;


namespace IntegrationTests.ApiTests;

[Collection("Integration Tests")]
public class SessionControllerTests : IntegrationTestBase
{
    public SessionControllerTests(IntegrationTestFixture fixture): base(fixture.Factory)
    {
    }
    private const string Endpoint = "/api/sessions";

    [Fact]
    public async Task Create_ShouldReturnCreated()
    {
        await ResetDatabaseAsync();
        var request = SessionTestData.CreateRequest(code: "2026-27",isCurrent: true);
        var response = await Client.PostAsJsonAsync(Endpoint, request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);            
        var result = await response.Content.ReadFromJsonAsync<SessionMstResponse>();
        Assert.NotNull(result);
        Assert.Equal("2026-27", result!.SessionCode);
        Assert.Equal("Academic Session 2026-27", result.SessionName);
        Assert.True(result.IsCurrent);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task Create_DuplicateCode_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();
        var firstRequest = new CreateSessionMstRequest
        {
            SessionCode = "2026-27",
            SessionName = "Academic Session 2026-27",
            StartDate = new DateOnly(2026, 4, 1),
            EndDate = new DateOnly(2027, 3, 31),
            IsCurrent = false
        };
        var firstResponse = await Client.PostAsJsonAsync(Endpoint, firstRequest);
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);
        var secondResponse = await Client.PostAsJsonAsync(Endpoint, firstRequest);
        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task Create_InvalidCode_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();
        var request = new CreateSessionMstRequest
        {
            SessionCode = "2026",
            SessionName = "Academic Session 2026-27",
            StartDate = new DateOnly(2026, 4, 1),
            EndDate = new DateOnly(2027, 3, 31),
            IsCurrent = false
        };
        var response = await Client.PostAsJsonAsync(Endpoint, request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCurrent_ShouldReturnCurrentSession()
    {
        await ResetDatabaseAsync();
        var request = new CreateSessionMstRequest
        {
            SessionCode = "2026-27",
            SessionName = "Academic Session 2026-27",
            StartDate = new DateOnly(2026, 4, 1),
            EndDate = new DateOnly(2027, 3, 31),
            IsCurrent = true
        };

        await Client.PostAsJsonAsync(Endpoint, request);
        var response = await Client.GetAsync($"{Endpoint}/current");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<SessionMstResponse>();
        Assert.NotNull(result);
        Assert.Equal("2026-27", result!.SessionCode);
        Assert.True(result.IsCurrent);
    }

    [Fact]
    public async Task GetAll_ShouldReturnSessions()
    {
        await ResetDatabaseAsync();
        await Client.PostAsJsonAsync(Endpoint, SessionTestData.CreateRequest(code: "2025-26"));
        await Client.PostAsJsonAsync(Endpoint, SessionTestData.CreateRequest(code: "2026-27"));
        var response = await Client.GetAsync(Endpoint);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<List<SessionMstResponse>>();
        Assert.NotNull(result);
        Assert.Equal(2, result!.Count);
    }

    [Fact]
    public async Task GetById_ShouldReturnSession()
    {
        await ResetDatabaseAsync();
        var createResponse = await Client.PostAsJsonAsync(Endpoint, SessionTestData.CreateRequest(code: "2026-27"));
        var created = await createResponse.Content.ReadFromJsonAsync<SessionMstResponse>();
        Assert.NotNull(created);
        var response = await Client.GetAsync($"{Endpoint}/{created!.SessionId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<SessionMstResponse>();
        Assert.NotNull(result);
        Assert.Equal(created.SessionId, result!.SessionId);
    }

    [Fact]
    public async Task GetById_NonExisting_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();
        var response = await Client.GetAsync($"{Endpoint}/999999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldReturnUpdatedSession()
    {
        await ResetDatabaseAsync();
        var createResponse = await Client.PostAsJsonAsync(Endpoint, SessionTestData.CreateRequest(code: "2026-27"));
        var created = await createResponse.Content.ReadFromJsonAsync<SessionMstResponse>();
        Assert.NotNull(created);
        var updateRequest = SessionTestData.CreateUpdateRequest(name: "Updated Academic Session 2026-27");
        var response = await Client.PutAsJsonAsync($"{Endpoint}/{created!.SessionId}", updateRequest);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<SessionMstResponse>();
        Assert.NotNull(result);
        Assert.Equal(created.SessionId, result!.SessionId);
        Assert.Equal("Updated Academic Session 2026-27", result!.SessionName);
        Assert.Equal("2026-27", result!.SessionCode);
    }

    [Fact]
    public async Task Create_EmptyCode_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();
        var request = SessionTestData.CreateRequest(code: string.Empty);
        var response = await Client.PostAsJsonAsync(Endpoint, request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_InvalidCode_ShouldReturnBadRequest_()
    {
        await ResetDatabaseAsync();
        var request = SessionTestData.CreateRequest(code: "2026");
        var response = await Client.PostAsJsonAsync(Endpoint, request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_InvalidDateRange_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();
        var request = SessionTestData.CreateRequest(code: "2026-27", startDate: new DateOnly(2027, 4, 1), endDate: new DateOnly(2027, 3, 31));
        var response = await Client.PostAsJsonAsync(Endpoint, request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_DuplicateCode_ShouldReturnConflict_()
    {
        await ResetDatabaseAsync();
        var request = SessionTestData.CreateRequest(code: "2026-27");
        var firstResponse = await Client.PostAsJsonAsync(Endpoint, request);
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);
        var secondResponse = await Client.PostAsJsonAsync(Endpoint, request);
        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task SetCurrent_ShouldSwitchCurrentSession()
    {
        await ResetDatabaseAsync();
        var firstResponse = await Client.PostAsJsonAsync(Endpoint, SessionTestData.CreateRequest(code: "2025-26", isCurrent: true));
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);
        var first = await firstResponse.Content.ReadFromJsonAsync<SessionMstResponse>();
        var secondResponse = await Client.PostAsJsonAsync(Endpoint, SessionTestData.CreateRequest(code: "2026-27", isCurrent: false));
        Assert.Equal(HttpStatusCode.Created, secondResponse.StatusCode);
        var second = await secondResponse.Content.ReadFromJsonAsync<SessionMstResponse>();
        var response = await Client.PatchAsync($"{Endpoint}/{second!.SessionId}/set-current", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var current = await Client.GetAsync($"{Endpoint}/current");
        var currentSession = await current.Content.ReadFromJsonAsync<SessionMstResponse>();        
        Assert.NotNull(currentSession);
        Assert.Equal(second.SessionId, currentSession!.SessionId);
    }

    [Fact]
    public async Task Activate_ShouldActivateSession()
    {
        await ResetDatabaseAsync();
        var createResponse = await Client.PostAsJsonAsync(Endpoint, SessionTestData.CreateRequest(code: "2026-27", isCurrent: false));
        var created =await createResponse.Content.ReadFromJsonAsync<SessionMstResponse>();
        var response = await Client.PatchAsync($"{Endpoint}/{created!.SessionId}/activate", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<SessionMstResponse>();
        Assert.True(result!.IsActive);
    }


    [Fact]
    public async Task Deactivate_ShouldDeactivateSession()
    {
        await ResetDatabaseAsync();
        var createResponse = await Client.PostAsJsonAsync(Endpoint, SessionTestData.CreateRequest(code: "2026-27", isCurrent: false));
        var created = await createResponse.Content.ReadFromJsonAsync<SessionMstResponse>();
        var response = await Client.PatchAsync($"{Endpoint}/{created!.SessionId}/deactivate", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<SessionMstResponse>();
        Assert.False(result!.IsActive);
    }

    [Fact]
    public async Task SetCurrent_ConcurrentRequests_ShouldLeaveOnlyOneCurrentSession()
    {
        await ResetDatabaseAsync();
        var session1Response = await Client.PostAsJsonAsync(Endpoint, SessionTestData.CreateRequest(code: "2025-26"));
        var session1 = await session1Response.Content.ReadFromJsonAsync<SessionMstResponse>();
        var session2Response = await Client.PostAsJsonAsync(Endpoint, SessionTestData.CreateRequest(code: "2026-27"));
        var session2 = await session2Response.Content.ReadFromJsonAsync<SessionMstResponse>();
        var session3Response = await Client.PostAsJsonAsync(Endpoint, SessionTestData.CreateRequest(code: "2027-28"));
        var session3 = await session3Response.Content.ReadFromJsonAsync<SessionMstResponse>();
        var task1 = Client.PatchAsync($"{Endpoint}/{session2!.SessionId}/set-current", null);
        var task2 = Client.PatchAsync($"{Endpoint}/{session3!.SessionId}/set-current", null);
        var responses = await Task.WhenAll(task1, task2);
        Assert.All(responses, response => Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Conflict));
        var allSessionsResponse = await Client.GetAsync(Endpoint);
        var sessions = await allSessionsResponse.Content.ReadFromJsonAsync<List<SessionMstResponse>>();
        Assert.NotNull(sessions);
        Assert.Equal(1, sessions!.Count(x => x.IsCurrent));
    }
}
