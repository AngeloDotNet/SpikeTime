using UGIdotNET.SpikeTime.TestcontainersApp.Migrations;
using UGIdotNET.SpikeTime.TestContainersApp.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddSqlServerDbContext<MyTasksDbContext>("SpikeTimeTestcontainers");

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
