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
public sealed class ClassControllerTests : IntegrationTestBase
{
    public ClassControllerTests(IntegrationTestFixture fixture) : base(fixture.Factory)
    {
    }
    private const string Endpoint = "/api/class";

    [Fact]
    public async Task Create_ShouldReturnCreated()
    {
        await ResetDatabaseAsync();

        var request =
            ClassTestData.CreateRequest(
                classCode: "CLASS-01",
                className: "Class 1",
                displayOrder: 1);

        var response =
            await Client.PostAsJsonAsync(
                Endpoint,
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var result =
            await response.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        Assert.NotNull(result);
        Assert.True(result!.ClassId > 0);
        Assert.Equal("CLASS-01", result.ClassCode);
        Assert.Equal("Class 1", result.ClassName);
        Assert.Equal(1, result.DisplayOrder);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task GetById_ShouldReturnClass()
    {
        await ResetDatabaseAsync();

        var createRequest =
            ClassTestData.CreateRequest();

        var createResponse =
            await Client.PostAsJsonAsync(
                Endpoint,
                createRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        Assert.NotNull(created);

        var response =
            await Client.GetAsync(
                $"{Endpoint}/{created!.ClassId}");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var result =
            await response.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        Assert.NotNull(result);
        Assert.Equal(
            created.ClassId,
            result!.ClassId);

        Assert.Equal(
            "CLASS-01",
            result.ClassCode);

        Assert.Equal(
            "Class 1",
            result.ClassName);

        Assert.Equal(
            1,
            result.DisplayOrder);
    }

    [Fact]
    public async Task GetAll_ShouldReturnClassesOrderedByDisplayOrder()
    {
        await ResetDatabaseAsync();

        var class1 =
            ClassTestData.CreateRequest(
                classCode: "CLASS-01",
                className: "Class 1",
                displayOrder: 1);

        var class2 =
            ClassTestData.CreateRequest(
                classCode: "CLASS-02",
                className: "Class 2",
                displayOrder: 2);

        var class3 =
            ClassTestData.CreateRequest(
                classCode: "CLASS-03",
                className: "Class 3",
                displayOrder: 3);

        await Client.PostAsJsonAsync(
            Endpoint,
            class1);

        await Client.PostAsJsonAsync(
            Endpoint,
            class2);

        await Client.PostAsJsonAsync(
            Endpoint,
            class3);

        var response =
            await Client.GetAsync(
                Endpoint);

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var result =
            await response.Content
                .ReadFromJsonAsync<List<ClassMstResponse>>();

        Assert.NotNull(result);
        Assert.Equal(3, result!.Count);

        Assert.Equal(
            "CLASS-01",
            result[0].ClassCode);

        Assert.Equal(
            "CLASS-02",
            result[1].ClassCode);

        Assert.Equal(
            "CLASS-03",
            result[2].ClassCode);
    }

    [Fact]
    public async Task Update_ShouldReturnUpdatedClass()
    {
        await ResetDatabaseAsync();

        var createRequest =
            ClassTestData.CreateRequest(
                classCode: "CLASS-01",
                className: "Class 1",
                displayOrder: 1);

        var createResponse =
            await Client.PostAsJsonAsync(
                Endpoint,
                createRequest);

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        Assert.NotNull(created);

        var updateRequest =
            ClassTestData.UpdateRequest(
                className: "Class One",
                displayOrder: 10);

        var response =
            await Client.PutAsJsonAsync(
                $"{Endpoint}/{created!.ClassId}",
                updateRequest);

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var result =
            await response.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        Assert.NotNull(result);

        Assert.Equal(
            created.ClassId,
            result!.ClassId);

        Assert.Equal(
            "CLASS-01",
            result.ClassCode);

        Assert.Equal(
            "Class One",
            result.ClassName);

        Assert.Equal(
            10,
            result.DisplayOrder);
    }

    [Fact]
    public async Task Create_WithDuplicateCode_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var firstRequest =
            ClassTestData.CreateRequest(
                classCode: "CLASS-01",
                className: "Class 1",
                displayOrder: 1);

        var firstResponse =
            await Client.PostAsJsonAsync(
                Endpoint,
                firstRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            firstResponse.StatusCode);

        var duplicateRequest =
            ClassTestData.CreateRequest(
                classCode: "CLASS-01",
                className: "Class One",
                displayOrder: 2);

        var response =
            await Client.PostAsJsonAsync(
                Endpoint,
                duplicateRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_WithDuplicateName_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var firstRequest =
            ClassTestData.CreateRequest(
                classCode: "CLASS-01",
                className: "Class 1",
                displayOrder: 1);

        var firstResponse =
            await Client.PostAsJsonAsync(
                Endpoint,
                firstRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            firstResponse.StatusCode);

        var duplicateRequest =
            ClassTestData.CreateRequest(
                classCode: "CLASS-02",
                className: "Class 1",
                displayOrder: 2);

        var response =
            await Client.PostAsJsonAsync(
                Endpoint,
                duplicateRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }


    [Fact]
    public async Task Create_WithDuplicateDisplayOrder_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var firstRequest =
            ClassTestData.CreateRequest(
                classCode: "CLASS-01",
                className: "Class 1",
                displayOrder: 1);

        var firstResponse =
            await Client.PostAsJsonAsync(
                Endpoint,
                firstRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            firstResponse.StatusCode);

        var duplicateRequest =
            ClassTestData.CreateRequest(
                classCode: "CLASS-02",
                className: "Class 2",
                displayOrder: 1);

        var response =
            await Client.PostAsJsonAsync(
                Endpoint,
                duplicateRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    /*
    [Fact]
    public async Task Update_WithDuplicateName_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var firstResponse =
            await Client.PostAsJsonAsync(
                Endpoint,
                ClassTestData.CreateRequest(
                    "CLASS-01",
                    "Class 1",
                    1));

        var first =
            await firstResponse.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        var secondResponse =
            await Client.PostAsJsonAsync(
                Endpoint,
                ClassTestData.CreateRequest(
                    "CLASS-02",
                    "Class 2",
                    2));

        var second =
            await secondResponse.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        Assert.NotNull(first);
        Assert.NotNull(second);

        var updateRequest =
            ClassTestData.UpdateRequest(
                "Class 1",
                2);

        var response =
            await Client.PutAsJsonAsync(
                $"{Endpoint}/{second!.ClassId}",
                updateRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }
    */

    [Fact]
    public async Task Update_WithDuplicateDisplayOrder_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();

        var firstResponse =
            await Client.PostAsJsonAsync(
                Endpoint,
                ClassTestData.CreateRequest(
                    "CLASS-01",
                    "Class 1",
                    1));

        var first =
            await firstResponse.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        var secondResponse =
            await Client.PostAsJsonAsync(
                Endpoint,
                ClassTestData.CreateRequest(
                    "CLASS-02",
                    "Class 2",
                    2));

        var second =
            await secondResponse.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        Assert.NotNull(first);
        Assert.NotNull(second);

        var updateRequest =
            ClassTestData.UpdateRequest(
                "Class Two",
                1);

        var response =
            await Client.PutAsJsonAsync(
                $"{Endpoint}/{second!.ClassId}",
                updateRequest);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_WithInvalidRequest_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();

        var request =
            ClassTestData.CreateRequest(
                classCode: "",
                className: "",
                displayOrder: 0);

        var response =
            await Client.PostAsJsonAsync(
                Endpoint,
                request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        var content =
            await response.Content.ReadAsStringAsync();

        Assert.Contains(
            "Class code is required.",
            content);

        Assert.Contains(
            "Class name is required.",
            content);

        Assert.Contains(
            "Display order must be greater than 0.",
            content);
    }

    [Fact]
    public async Task GetById_WhenClassDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var response =
            await Client.GetAsync(
                $"{Endpoint}/999999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Update_WhenClassDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var request =
            ClassTestData.UpdateRequest();

        var response =
            await Client.PutAsJsonAsync(
                $"{Endpoint}/999999",
                request);

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Deactivate_ShouldReturnNoContent()
    {
        await ResetDatabaseAsync();

        var createResponse =
            await Client.PostAsJsonAsync(
                Endpoint,
                ClassTestData.CreateRequest());

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        Assert.NotNull(created);

        var response =
            await Client.PatchAsync(
                $"{Endpoint}/{created!.ClassId}/deactivate",
                null);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        var getResponse =
            await Client.GetAsync(
                $"{Endpoint}/{created.ClassId}");

        var result =
            await getResponse.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        Assert.NotNull(result);
        Assert.False(result!.IsActive);
    }

    [Fact]
    public async Task Activate_ShouldReturnNoContent()
    {
        await ResetDatabaseAsync();

        var createResponse =
            await Client.PostAsJsonAsync(
                Endpoint,
                ClassTestData.CreateRequest());

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        Assert.NotNull(created);

        await Client.PatchAsync(
            $"{Endpoint}/{created!.ClassId}/deactivate",
            null);

        var response =
            await Client.PatchAsync(
                $"{Endpoint}/{created.ClassId}/activate",
                null);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        var getResponse =
            await Client.GetAsync(
                $"{Endpoint}/{created.ClassId}");

        var result =
            await getResponse.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        Assert.NotNull(result);
        Assert.True(result!.IsActive);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent()
    {
        await ResetDatabaseAsync();

        var createResponse =
            await Client.PostAsJsonAsync(
                Endpoint,
                ClassTestData.CreateRequest());

        var created =
            await createResponse.Content
                .ReadFromJsonAsync<ClassMstResponse>();

        Assert.NotNull(created);

        var deleteResponse =
            await Client.DeleteAsync(
                $"{Endpoint}/{created!.ClassId}");

        Assert.Equal(
            HttpStatusCode.NoContent,
            deleteResponse.StatusCode);

        var getResponse =
            await Client.GetAsync(
                $"{Endpoint}/{created.ClassId}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            getResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenClassDoesNotExist_ShouldReturnNotFound()
    {
        await ResetDatabaseAsync();

        var response =
            await Client.DeleteAsync(
                $"{Endpoint}/999999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenClassCodeExceedsMaxLength_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();

        var request =
            ClassTestData.CreateRequest(
                classCode: new string('A', 21),
                className: "Class 1",
                displayOrder: 1);

        var response =
            await Client.PostAsJsonAsync(
                Endpoint,
                request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenClassNameExceedsMaxLength_ShouldReturnBadRequest()
    {
        await ResetDatabaseAsync();

        var request =
            ClassTestData.CreateRequest(
                classCode: "CLASS-01",
                className: new string('A', 101),
                displayOrder: 1);

        var response =
            await Client.PostAsJsonAsync(
                Endpoint,
                request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    /*
    [Fact]
    public async Task Create_ConcurrentDuplicateCode_ShouldReturnOneCreatedAndOneConflict()
    {
        await ResetDatabaseAsync();

        var request1 =
            ClassTestData.CreateRequest(
                classCode: "CLASS-CONCURRENT",
                className: "Concurrent Class 1",
                displayOrder: 1);

        var request2 =
            ClassTestData.CreateRequest(
                classCode: "CLASS-CONCURRENT",
                className: "Concurrent Class 2",
                displayOrder: 2);

        var task1 =
            Client.PostAsJsonAsync(
                Endpoint,
                request1);

        var task2 =
            Client.PostAsJsonAsync(
                Endpoint,
                request2);

        var responses =
            await Task.WhenAll(
                task1,
                task2);

        var statuses =
            responses
                .Select(x => x.StatusCode)
                .OrderBy(x => x)
                .ToList();

        Assert.Contains(
            HttpStatusCode.Created,
            statuses);

        Assert.Contains(
            HttpStatusCode.Conflict,
            statuses);

        Assert.Equal(
            1,
            statuses.Count(
                x => x == HttpStatusCode.Created));

        Assert.Equal(
            1,
            statuses.Count(
                x => x == HttpStatusCode.Conflict));
    }
    */

}
