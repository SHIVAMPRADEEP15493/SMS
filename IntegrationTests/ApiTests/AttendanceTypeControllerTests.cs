using IntegrationTests.Infrastructure;
using IntegrationTests.TestData;
using SMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace IntegrationTests.ApiTests;

[Collection("Integration Tests")]
public sealed class AttendanceTypeControllerTests : IntegrationTestBase
{
    public AttendanceTypeControllerTests(
        IntegrationTestFixture fixture)
        : base(fixture.Factory)
    {
    }

    private const string Endpoint = "/api/AttendanceTypes";
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    private async Task<AttendanceTypeMstResponse> CreateAttendanceTypeAsync(
        string code = "P",
        string name = "Present")
    {
        var request = new CreateAttendanceTypeMstRequest(
            code,
            name);

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<AttendanceTypeMstResponse>(JsonOptions);

        Assert.NotNull(result);

        return result;
    }

    [Fact]
    public async Task Create_ShouldReturnCreated()
    {
        await ResetDatabaseAsync();

        var request = new CreateAttendanceTypeMstRequest(
            "P",
            "Present");

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content
            .ReadFromJsonAsync<AttendanceTypeMstResponse>(JsonOptions);

        Assert.NotNull(result);
        Assert.True(result.AttendanceTypeId > 0);
        Assert.Equal("P", result.AttendanceTypeCode);
        Assert.Equal("Present", result.AttendanceTypeName);
        Assert.True(result.IsActive);
        Assert.NotEqual(default, result.CreatedAt);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task Create_DuplicateCode_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        await CreateAttendanceTypeAsync("P", "Present");

        var duplicateRequest = new CreateAttendanceTypeMstRequest(
            "P",
            "Present Duplicate");

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            duplicateRequest);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Theory]
    [InlineData("", "Present")]
    [InlineData("P", "")]
    [InlineData("TOOLONGCODE", "Present")]
    [InlineData("P@", "Present")]
    public async Task Create_InvalidRequest_ShouldReturnBadRequest(
        string code,
        string name)
    {
        await ResetDatabaseAsync();

        var request = new CreateAttendanceTypeMstRequest(code, name);

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ExistingAttendanceType_ShouldReturnOk()
    {
        await ResetDatabaseAsync();

        var created = await CreateAttendanceTypeAsync("A", "Absent");

        var response = await Client.GetAsync(
            $"{Endpoint}/{created.AttendanceTypeId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content
            .ReadFromJsonAsync<AttendanceTypeMstResponse>(JsonOptions);

        Assert.NotNull(result);
        Assert.Equal(created.AttendanceTypeId, result.AttendanceTypeId);
        Assert.Equal("A", result.AttendanceTypeCode);
        Assert.Equal("Absent", result.AttendanceTypeName);
    }

    [Fact]
    public async Task GetById_NonExistingId_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync(
            $"{Endpoint}/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAttendanceTypes()
    {
        await ResetDatabaseAsync();

        await CreateAttendanceTypeAsync("P", "Present");
        await CreateAttendanceTypeAsync("A", "Absent");
        await CreateAttendanceTypeAsync("M", "Medical Leave");
        await CreateAttendanceTypeAsync("O", "On Duty");

        var response = await Client.GetAsync(
            Endpoint);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content
            .ReadFromJsonAsync<List<AttendanceTypeMstResponse>>(
                JsonOptions);

        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
        Assert.Contains(result, x => x.AttendanceTypeCode == "P");
        Assert.Contains(result, x => x.AttendanceTypeCode == "A");
        Assert.Contains(result, x => x.AttendanceTypeCode == "M");
        Assert.Contains(result, x => x.AttendanceTypeCode == "O");
    }

    [Fact]
    public async Task Update_ExistingAttendanceType_ShouldReturnUpdatedRecord()
    {
        await ResetDatabaseAsync();

        var created = await CreateAttendanceTypeAsync("P", "Present");

        var request = new UpdateAttendanceTypeMstRequest(
            "PR",
            "Present Regular");

        var response = await Client.PutAsJsonAsync(
            $"{Endpoint}/{created.AttendanceTypeId}",
            request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content
            .ReadFromJsonAsync<AttendanceTypeMstResponse>(JsonOptions);

        Assert.NotNull(result);
        Assert.Equal(created.AttendanceTypeId, result.AttendanceTypeId);
        Assert.Equal("PR", result.AttendanceTypeCode);
        Assert.Equal("Present Regular", result.AttendanceTypeName);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task Update_DuplicateCode_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var present = await CreateAttendanceTypeAsync("P", "Present");
        await CreateAttendanceTypeAsync("A", "Absent");

        var request = new UpdateAttendanceTypeMstRequest(
            "P",
            "Absent");

        var response = await Client.PutAsJsonAsync(
            $"{Endpoint}/{(await Client
                .GetFromJsonAsync<List<AttendanceTypeMstResponse>>(
                    Endpoint, JsonOptions))!
                .Single(x => x.AttendanceTypeCode == "A")
                .AttendanceTypeId}",
            request);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        //await ResetDatabaseAsync();

        //await CreateAttendanceTypeAsync("P", "Present");
        //var absent = await CreateAttendanceTypeAsync("A", "Absent");

        //var request = new UpdateAttendanceTypeMstRequest(
        //    "P",
        //    "Absent");

        //var response = await Client.PutAsJsonAsync(
        //    $"/{Endpoint}/{absent.AttendanceTypeId}",
        //    request);

        //Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ActiveOnly_ShouldExcludeInactiveTypes()
    {
        await ResetDatabaseAsync();

        var present = await CreateAttendanceTypeAsync("P", "Present");
        await CreateAttendanceTypeAsync("A", "Absent");

        var deactivateResponse = await Client.PatchAsync(
            $"{Endpoint}/{present.AttendanceTypeId}/deactivate",
            content: null);

        Assert.Equal(HttpStatusCode.OK, deactivateResponse.StatusCode);

        var response = await Client.GetAsync(
            $"{Endpoint}?activeOnly=true");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content
            .ReadFromJsonAsync<List<AttendanceTypeMstResponse>>(
                JsonOptions);

        Assert.NotNull(result);
        Assert.DoesNotContain(
            result,
            x => x.AttendanceTypeId == present.AttendanceTypeId);
        Assert.Contains(
            result,
            x => x.AttendanceTypeCode == "A");
    }

    [Fact]
    public async Task Activate_DeactivatedType_ShouldReturnActiveRecord()
    {
        await ResetDatabaseAsync();

        var created = await CreateAttendanceTypeAsync("M", "Medical Leave");

        var deactivateResponse = await Client.PatchAsync(
            $"{Endpoint}/{created.AttendanceTypeId}/deactivate",
            content: null);

        Assert.Equal(HttpStatusCode.OK, deactivateResponse.StatusCode);

        var activateResponse = await Client.PatchAsync(
            $"{Endpoint}/{created.AttendanceTypeId}/activate",
            content: null);

        Assert.Equal(HttpStatusCode.OK, activateResponse.StatusCode);

        var result = await activateResponse.Content
            .ReadFromJsonAsync<AttendanceTypeMstResponse>(JsonOptions);

        Assert.NotNull(result);
        Assert.True(result.IsActive);
        Assert.NotNull(result.UpdatedAt);
    }
}
