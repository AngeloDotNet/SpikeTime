var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("spiketime-testcontainers-sql")
    .WithDataVolume("spiketime-testcontainers-data");

var db = sql.AddDatabase("SpikeTimeTestcontainers");

var redis = builder.AddRedis("spiketime-testcontainers-redis");

var webapi = builder.AddProject<Projects.UGIdotNET_SpikeTime_TestcontainersApp>("spiketime-testcontainers-webapi")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(redis)
    .WaitFor(redis)
    .WithUrls(ctx =>
    {
        foreach (var url in ctx.Urls)
        {
            url.DisplayText = $"Scalar UI ({url.Endpoint?.Scheme?.ToUpper()})";
        }
    });

var migrations = builder.AddProject<Projects.UGIdotNET_SpikeTime_TestcontainersApp_Migrations>("spiketime-testcontainers-migrations")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
