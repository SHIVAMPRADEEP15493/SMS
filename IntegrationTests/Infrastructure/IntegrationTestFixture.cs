using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Infrastructure;
public sealed class IntegrationTestFixture : IAsyncLifetime
{
    public CustomWebApplicationFactory Factory { get; }
    public IntegrationTestFixture()
    {
        Factory = new CustomWebApplicationFactory();
    }
    public async Task InitializeAsync()
    {
        // Force application startup.
        _ = Factory.CreateClient();
        await Factory.InitializeAsync();
    }
    public async Task DisposeAsync()
    {
        Factory.Dispose();
        await Task.CompletedTask;
    }
}
