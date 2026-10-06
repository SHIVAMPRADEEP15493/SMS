using IntegrationTests.TestData;
using SMS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace IntegrationTests.Infrastructure;
public abstract class IntegrationTestBase
{
    protected CustomWebApplicationFactory Factory { get; }
    protected HttpClient Client { get; }
    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }
    protected async Task ResetDatabaseAsync()
    {
        await Factory.ResetDatabaseAsync();
    }

    protected async Task<long> CreateSchoolAsync(
    string schoolCode = "SCH001")
    {
        var request = SchoolTestData.CreateRequest(
            code: schoolCode);

        var response =
            await Client.PostAsJsonAsync(
                "/api/schools",
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var result =
            await response.Content
                .ReadFromJsonAsync<SchoolMstDto>();

        Assert.NotNull(result);

        return result!.Id;
    }

}
