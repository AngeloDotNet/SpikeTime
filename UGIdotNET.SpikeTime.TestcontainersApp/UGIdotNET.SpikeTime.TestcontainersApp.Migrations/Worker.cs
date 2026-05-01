using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using UGIdotNET.SpikeTime.TestContainersApp.Data;

namespace UGIdotNET.SpikeTime.TestcontainersApp.Migrations;

public class Worker(
    ILogger<Worker> logger,
    IHostApplicationLifetime hostApplicationLifetime,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private static readonly ActivitySource _activitySource = new("UGIdotNET.SpikeTime.Testcontainers.Migrations");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = _activitySource.StartActivity("Create database");

        using var scope = serviceScopeFactory.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<MyTasksDbContext>();
        await EnsureDatabaseIsCreatedAsync(db, stoppingToken);

        logger.LogInformation("Applying all pending migrations...");
        await db.Database.MigrateAsync(stoppingToken).ConfigureAwait(false);
        logger.LogInformation("Migrations applied correctly!");

        hostApplicationLifetime.StopApplication();
    }

    private async Task EnsureDatabaseIsCreatedAsync(MyTasksDbContext db, CancellationToken stoppingToken)
    {
        var creator = db.GetService<IRelationalDatabaseCreator>();
        if (await creator.ExistsAsync(stoppingToken))
        {
            logger.LogInformation("Database already exist!");
            return;
        }

        logger.LogInformation("Creating database...");
        await creator.CreateAsync(stoppingToken);
        logger.LogInformation("Database created successfully.");
    }
}
