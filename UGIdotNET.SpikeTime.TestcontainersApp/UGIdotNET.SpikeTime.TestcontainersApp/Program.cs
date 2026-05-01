using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Scalar.AspNetCore;
using System.Text.Json;
using UGIdotNET.SpikeTime.TestContainersApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<MyTasksDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SpikeTimeTestcontainers")));

builder.AddRedisDistributedCache("spiketime-testcontainers-redis");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

var myTasksGroup = app.MapGroup("api/tasks");

myTasksGroup.MapGet("", GetAllTasks)
    .WithName(nameof(GetAllTasks));

myTasksGroup.MapPost("", CreateNewTask)
    .WithName(nameof(CreateNewTask));

myTasksGroup.MapGet("{id}", GetTaskDetail)
    .WithName(nameof(GetTaskDetail));

app.Run();

static async Task<Ok<MyTask[]>> GetAllTasks(MyTasksDbContext db)
{
    var tasks = await db.MyTasks.Where(t => t.CompletedAt == null).ToArrayAsync();
    return TypedResults.Ok(tasks);
}

static async Task<CreatedAtRoute<MyTask>> CreateNewTask(MyTasksDbContext db, [FromBody] MyTask task)
{
    task.Id = Guid.NewGuid();

    db.Add(task);
    await db.SaveChangesAsync();

    return TypedResults.CreatedAtRoute(task, nameof(GetTaskDetail), new { id = task.Id });
}

static async Task<Results<Ok<MyTask>, NotFound>> GetTaskDetail(MyTasksDbContext db, IDistributedCache cache, Guid id)
{
    var cacheKey = $"tasks:{id}";

    var cached = await cache.GetStringAsync(cacheKey);
    if (cached is not null)
    {
        return TypedResults.Ok(JsonSerializer.Deserialize<MyTask>(cached)!);
    }

    var task = await db.MyTasks.SingleOrDefaultAsync(t => t.Id == id);
    if (task is null)
    {
        return TypedResults.NotFound();
    }

    await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(task));

    return TypedResults.Ok(task);
}
