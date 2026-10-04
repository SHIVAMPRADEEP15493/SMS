using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using SMS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Infrastructure;
public sealed class DatabaseResetter
{
    /*
    private readonly ApplicationDbContext _dbContext;
    private Respawner? _respawner;

    public DatabaseResetter(
        ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InitializeAsync()
    {
        await _dbContext.Database.OpenConnectionAsync();

        _respawner = await Respawner.CreateAsync(
            _dbContext.Database.GetDbConnection(),
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"],
                TablesToIgnore =
                [
                    "__EFMigrationsHistory"
                ]
            });
    }
    */
    private readonly string _connectionString;
    private Respawner? _respawner;
    public DatabaseResetter(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        _respawner = await Respawner
            .CreateAsync(connection, 
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude =
                [
                    "public"
                ],
                TablesToIgnore =
                [
                    "__EFMigrationsHistory"
                ]
            });
    }
    public async Task ResetAsync()
    {
        if (_respawner is null)
        {
            throw new InvalidOperationException("DatabaseResetter has not been initialized.");
        }
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await _respawner.ResetAsync(connection);
    }

    /*
    public async Task ResetAsync()
    {
        if (_respawner is null)
        {
            throw new InvalidOperationException(
                "DatabaseResetter has not been initialized.");
        }

        await _respawner.ResetAsync(
            _dbContext.Database.GetDbConnection());
    }
    */
}
