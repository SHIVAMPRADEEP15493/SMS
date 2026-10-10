using IntegrationTests.Infrastructure;
using SMS.Application.DTOs.Create;
using SMS.Application.DTOs.Update;
using SMS.Application.DTOs.Response;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.ApiTests;

[Collection("Integration Tests")]
public class SectionControllerTests : IntegrationTestBase
{
    public SectionControllerTests(IntegrationTestFixture fixture) : base(fixture.Factory)
    {
    }

    private const string Endpoint = "/api/sections";

    [Fact]
    public async Task Create_ShouldReturnCreated()
    {
        await ResetDatabaseAsync();

        var request = new CreateSectionMstRequest
        {
            SectionCode = "A",
            SectionName = "Section A",
            DisplayOrder = 1
        };

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var section =
            await response.Content
                .ReadFromJsonAsync<SectionMstResponse>();

        Assert.NotNull(section);
        Assert.True(section!.SectionId > 0);
        Assert.Equal("A", section.SectionCode);
        Assert.Equal("Section A", section.SectionName);
        Assert.Equal(1, section.DisplayOrder);
        Assert.True(section.IsActive);
    }

    [Fact]
    public async Task Create_WithDuplicateCode_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var firstRequest = new CreateSectionMstRequest
        {
            SectionCode = "A",
            SectionName = "Section A",
            DisplayOrder = 1
        };

        var secondRequest = new CreateSectionMstRequest
        {
            SectionCode = "A",
            SectionName = "Section B",
            DisplayOrder = 2
        };

        var firstResponse = await Client.PostAsJsonAsync(
            Endpoint,
            firstRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            firstResponse.StatusCode);

        var secondResponse = await Client.PostAsJsonAsync(
            Endpoint,
            secondRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            secondResponse.StatusCode);
    }

    [Fact]
    public async Task Create_WithDuplicateName_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var firstRequest = new CreateSectionMstRequest
        {
            SectionCode = "A",
            SectionName = "Section A",
            DisplayOrder = 1
        };

        var secondRequest = new CreateSectionMstRequest
        {
            SectionCode = "B",
            SectionName = "Section A",
            DisplayOrder = 2
        };

        var firstResponse = await Client.PostAsJsonAsync(
            Endpoint,
            firstRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            firstResponse.StatusCode);

        var secondResponse = await Client.PostAsJsonAsync(
            Endpoint,
            secondRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            secondResponse.StatusCode);
    }

    [Fact]
    public async Task Create_WithDuplicateDisplayOrder_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var firstRequest = new CreateSectionMstRequest
        {
            SectionCode = "A",
            SectionName = "Section A",
            DisplayOrder = 1
        };

        var secondRequest = new CreateSectionMstRequest
        {
            SectionCode = "B",
            SectionName = "Section B",
            DisplayOrder = 1
        };

        var firstResponse = await Client.PostAsJsonAsync(
            Endpoint,
            firstRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            firstResponse.StatusCode);

        var secondResponse = await Client.PostAsJsonAsync(
            Endpoint,
            secondRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            secondResponse.StatusCode);
    }

    [Fact]
    public async Task Create_WithInvalidRequest_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();

        var request = new CreateSectionMstRequest
        {
            SectionCode = "",
            SectionName = "",
            DisplayOrder = 0
        };

        var response = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        var content =
            await response.Content.ReadAsStringAsync();

        Assert.Contains(
            "Section code is required.",
            content);

        Assert.Contains(
            "Section name is required.",
            content);

        Assert.Contains(
            "Display order must be greater than 0.",
            content);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        await ResetDatabaseAsync();

        var createRequest = new CreateSectionMstRequest
        {
            SectionCode = "A",
            SectionName = "Section A",
            DisplayOrder = 1
        };

        var createResponse = await Client.PostAsJsonAsync(
            Endpoint,
            createRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<SectionMstResponse>();

        Assert.NotNull(created);

        var response = await Client.GetAsync(
            $"{Endpoint}/{created!.SectionId}");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var section =
            await response.Content
                .ReadFromJsonAsync<SectionMstResponse>();

        Assert.NotNull(section);
        Assert.Equal(
            created.SectionId,
            section!.SectionId);

        Assert.Equal(
            "A",
            section.SectionCode);

        Assert.Equal(
            "Section A",
            section.SectionName);
    }

    [Fact]
    public async Task GetById_WhenSectionDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync(
            $"{Endpoint}/999999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ShouldReturnSectionsOrderedByDisplayOrder()
    {
        await ResetDatabaseAsync();

        var sectionA = new CreateSectionMstRequest
        {
            SectionCode = "A",
            SectionName = "Section A",
            DisplayOrder = 2
        };

        var sectionB = new CreateSectionMstRequest
        {
            SectionCode = "B",
            SectionName = "Section B",
            DisplayOrder = 1
        };

        var responseA = await Client.PostAsJsonAsync(
            Endpoint,
            sectionA);

        var responseB = await Client.PostAsJsonAsync(
            Endpoint,
            sectionB);

        Assert.Equal(
            HttpStatusCode.Created,
            responseA.StatusCode);

        Assert.Equal(
            HttpStatusCode.Created,
            responseB.StatusCode);

        var response = await Client.GetAsync(
            Endpoint);

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var sections =
            await response.Content
                .ReadFromJsonAsync<List<SectionMstResponse>>();

        Assert.NotNull(sections);

        Assert.Equal(2, sections!.Count);

        Assert.Equal(
            "B",
            sections[0].SectionCode);

        Assert.Equal(
            1,
            sections[0].DisplayOrder);

        Assert.Equal(
            "A",
            sections[1].SectionCode);

        Assert.Equal(
            2,
            sections[1].DisplayOrder);
    }

    [Fact]
    public async Task Update_ShouldReturnOk()
    {
        await ResetDatabaseAsync();

        var createRequest = new CreateSectionMstRequest
        {
            SectionCode = "A",
            SectionName = "Section A",
            DisplayOrder = 1
        };

        var createResponse = await Client.PostAsJsonAsync(
            Endpoint,
            createRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<SectionMstResponse>();

        Assert.NotNull(created);

        var updateRequest = new UpdateSectionMstRequest
        {
            SectionName = "Section Alpha",
            DisplayOrder = 2
        };

        var response = await Client.PutAsJsonAsync(
            $"{Endpoint}/{created!.SectionId}",
            updateRequest);

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var updated =
            await response.Content
                .ReadFromJsonAsync<SectionMstResponse>();

        Assert.NotNull(updated);

        Assert.Equal(
            created.SectionId,
            updated!.SectionId);

        Assert.Equal(
            "A",
            updated.SectionCode);

        Assert.Equal(
            "Section Alpha",
            updated.SectionName);

        Assert.Equal(
            2,
            updated.DisplayOrder);
    }

    [Fact]
    public async Task Update_WhenSectionDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var request = new UpdateSectionMstRequest
        {
            SectionName = "Section A",
            DisplayOrder = 1
        };

        var response = await Client.PutAsJsonAsync(
            $"{Endpoint}/999999",
            request);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Update_WithDuplicateName_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var firstRequest = new CreateSectionMstRequest
        {
            SectionCode = "A",
            SectionName = "Section A",
            DisplayOrder = 1
        };

        var secondRequest = new CreateSectionMstRequest
        {
            SectionCode = "B",
            SectionName = "Section B",
            DisplayOrder = 2
        };

        await Client.PostAsJsonAsync(
            Endpoint,
            firstRequest);

        var secondResponse = await Client.PostAsJsonAsync(
            Endpoint,
            secondRequest);

        var secondSection =
            await secondResponse.Content
                .ReadFromJsonAsync<SectionMstResponse>();

        Assert.NotNull(secondSection);

        var updateRequest = new UpdateSectionMstRequest
        {
            SectionName = "Section A",
            DisplayOrder = 2
        };

        var response = await Client.PutAsJsonAsync(
            $"{Endpoint}/{secondSection!.SectionId}",
            updateRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task Update_WithDuplicateDisplayOrder_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var firstRequest = new CreateSectionMstRequest
        {
            SectionCode = "A",
            SectionName = "Section A",
            DisplayOrder = 1
        };

        var secondRequest = new CreateSectionMstRequest
        {
            SectionCode = "B",
            SectionName = "Section B",
            DisplayOrder = 2
        };

        await Client.PostAsJsonAsync(
            Endpoint,
            firstRequest);

        var secondResponse = await Client.PostAsJsonAsync(
            Endpoint,
            secondRequest);

        var secondSection =
            await secondResponse.Content
                .ReadFromJsonAsync<SectionMstResponse>();

        Assert.NotNull(secondSection);

        var updateRequest = new UpdateSectionMstRequest
        {
            SectionName = "Section B Updated",
            DisplayOrder = 1
        };

        var response = await Client.PutAsJsonAsync(
            $"{Endpoint}/{secondSection!.SectionId}",
            updateRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task Update_WithInvalidRequest_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();

        var createRequest = new CreateSectionMstRequest
        {
            SectionCode = "A",
            SectionName = "Section A",
            DisplayOrder = 1
        };

        var createResponse = await Client.PostAsJsonAsync(
            Endpoint,
            createRequest);

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<SectionMstResponse>();

        Assert.NotNull(created);

        var updateRequest = new UpdateSectionMstRequest
        {
            SectionName = "",
            DisplayOrder = 0
        };

        var response = await Client.PutAsJsonAsync(
            $"{Endpoint}/{created!.SectionId}",
            updateRequest);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        var content =
            await response.Content.ReadAsStringAsync();

        Assert.Contains(
            "Section name is required.",
            content);

        Assert.Contains(
            "Display order must be greater than 0.",
            content);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent()
    {
        await ResetDatabaseAsync();

        var request = new CreateSectionMstRequest
        {
            SectionCode = "A",
            SectionName = "Section A",
            DisplayOrder = 1
        };

        var createResponse = await Client.PostAsJsonAsync(
            Endpoint,
            request);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<SectionMstResponse>();

        Assert.NotNull(created);

        var response = await Client.DeleteAsync(
            $"{Endpoint}/{created!.SectionId}");

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenSectionDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var response = await Client.DeleteAsync(
            $"{Endpoint}/999999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
