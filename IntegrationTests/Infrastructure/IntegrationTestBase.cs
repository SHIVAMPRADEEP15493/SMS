using System;
using System.Collections.Generic;
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

}
