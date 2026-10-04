using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;


namespace IntegrationTests.Infrastructure;
public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    /*
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }
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

            dbContext.Database.EnsureCreated();
        });
    }
    */

    /*
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
        //    "DELETE FROM SchoolMst;");
        // Option 1: Use schema-qualified table name
        await dbContext.Database.ExecuteSqlRawAsync(
            "DELETE FROM public.\"SchoolMst\";");

        // OR Option 2: Use lowercase (PostgreSQL default)
        // await dbContext.Database.ExecuteSqlRawAsync(
        //     "DELETE FROM public.sms_school_mst;");
    }
    */
    private DatabaseResetter? _databaseResetter;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile(
                    "appsettings.Testing.json",
                    optional: false,
                    reloadOnChange: false);
            });
    }
    /*
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

            var configuration =
               scope.ServiceProvider
                   .GetRequiredService<IConfiguration>();

            var connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Test database connection string was not found.");
            }

            _databaseResetter =
                new DatabaseResetter(connectionString);
        });
    }

    public async Task InitializeDatabaseResetAsync()
    {
        if (_databaseResetter is null)
        {
            throw new InvalidOperationException(
                "Database resetter has not been initialized.");
        }

        await _databaseResetter.InitializeAsync();
    }
    */
    public async Task ResetDatabaseAsync()
    {
        if (_databaseResetter is null)
        {
            throw new InvalidOperationException("Database resetter has not been initialized.");
        }
        await _databaseResetter.ResetAsync();
    }
    public async Task InitializeAsync()
    {
        /*
        using var scope = Services.CreateScope();

        var configuration =
            scope.ServiceProvider
                .GetRequiredService<IConfiguration>();

        var connectionString =
            configuration.GetConnectionString(
                "DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Test database connection string was not found.");
        }

        _databaseResetter =
            new DatabaseResetter(connectionString);

        await _databaseResetter.InitializeAsync();
        */

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Apply all EF Core migrations to the test database.
        await dbContext.Database.MigrateAsync();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Test database connection string was not found.");
        }
        _databaseResetter = new DatabaseResetter(connectionString);
        await _databaseResetter.InitializeAsync();
    }
}
