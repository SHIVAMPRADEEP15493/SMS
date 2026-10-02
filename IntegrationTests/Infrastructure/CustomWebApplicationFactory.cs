using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SMS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;


namespace IntegrationTests.Infrastructure;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    //protected override void ConfigureWebHost(IWebHostBuilder builder)
    //{
    //    builder.UseEnvironment("Testing");
    //}
    //protected override void ConfigureWebHost(
    //    IWebHostBuilder builder)
    //{
    //    builder.UseEnvironment("Testing");

    //    builder.ConfigureServices(services =>
    //    {
    //        using var serviceProvider =
    //            services.BuildServiceProvider();

    //        using var scope =
    //            serviceProvider.CreateScope();

    //        var dbContext =
    //            scope.ServiceProvider
    //                .GetRequiredService<ApplicationDbContext>();

    //        dbContext.Database.EnsureCreated();
    //    });
    //}

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            using var serviceProvider =
                services.BuildServiceProvider();

            using var scope =
                serviceProvider.CreateScope();

            var dbContext =
                scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        });
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope =
            Services.CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        //await dbContext.Database.ExecuteSqlRawAsync(
        //    "DELETE FROM SMS_School_Mst;");
        // Option 1: Use schema-qualified table name
        await dbContext.Database.ExecuteSqlRawAsync(
            "DELETE FROM public.\"SMS_School_Mst\";");

        // OR Option 2: Use lowercase (PostgreSQL default)
        // await dbContext.Database.ExecuteSqlRawAsync(
        //     "DELETE FROM public.sms_school_mst;");
    }
}
