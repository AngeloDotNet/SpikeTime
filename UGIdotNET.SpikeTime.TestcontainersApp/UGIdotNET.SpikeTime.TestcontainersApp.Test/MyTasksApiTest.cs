using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Net;
using System.Text.Json;
using UGIdotNET.SpikeTime.TestContainersApp.Data;

namespace UGIdotNET.SpikeTime.TestcontainersApp.Test;

public class MyTasksApiTest : IClassFixture<TestContainersFixture>
{
    private readonly TestContainersFixture _fixture;

    private Guid myTaskId = Guid.NewGuid();

    public MyTasksApiTest(TestContainersFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task<WebApplicationFactory<Program>> CreateWebApplicationFactoryAsync()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services
                        .FirstOrDefault(s => s.ServiceType == typeof(DbContextOptions<MyTasksDbContext>));

                    if (dbContextDescriptor is not null)
                    {
                        services.Remove(dbContextDescriptor);
                    }

                    services.AddDbContext<MyTasksDbContext>(
                        options => options.UseSqlServer(_fixture.SqlServerConnectionString));

                    var redisMultiplexerDescriptor = services
                        .FirstOrDefault(s => s.ServiceType == typeof(IConnectionMultiplexer));

                    if (redisMultiplexerDescriptor is not null)
                    {
                        services.Remove(redisMultiplexerDescriptor);
                    }

                    services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(_fixture.RedisConnectionString));
                });
            });

        using var scope = webApplicationFactory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<MyTasksDbContext>();
        await EnsureDatabaseIsCreatedAsync(db);

        await db.Database.MigrateAsync().ConfigureAwait(false);

        return webApplicationFactory;
    }

    private async Task EnsureDatabaseIsCreatedAsync(MyTasksDbContext db)
    {
        var creator = db.GetService<IRelationalDatabaseCreator>();
        if (await creator.ExistsAsync())
        {
            return;
        }

        await creator.CreateAsync();
    }

    [Fact]
    public async Task GetAllTasks_Should_Return_Ok_Status_Code()
    {
        using var app = await CreateWebApplicationFactoryAsync();
        await InitializeGetTasksDataAsync(app);

        using var client = app.CreateClient();

        var response = await client.GetAsync("api/tasks");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var myTasks = await response.Content.ReadFromJsonAsync<MyTask[]>() ?? [];
        
        Assert.Single(myTasks);
        Assert.Null(myTasks[0].CompletedAt);
        Assert.Equal("Spike time Testcontainers", myTasks[0].Title);
    }

    [Fact]
    public async Task CreateTask_Should_Create_New_Task()
    {
        using var app = await CreateWebApplicationFactoryAsync();

        using var client = app.CreateClient();

        var myTask = new MyTask { Id = Guid.NewGuid(), Title = "Scrivere i test di integrazione" };

        var response = await client.PostAsJsonAsync("api/tasks", myTask);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var taskFromResponse = await response.Content.ReadFromJsonAsync<MyTask>();

        using var scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<MyTasksDbContext>();

        var taskFound = await db.MyTasks.SingleOrDefaultAsync(t => t.Id == taskFromResponse!.Id);

        Assert.NotNull(taskFound);
        Assert.Equal("Scrivere i test di integrazione", taskFound.Title);
    }

    [Fact]
    public async Task GetTaskDetail_Should_Return_Item_From_Cache_If_Present()
    {
        using var app = await CreateWebApplicationFactoryAsync();

        var cache = app.Services.GetRequiredService<IDistributedCache>();
        var cacheKey = $"tasks:{myTaskId}";
        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(new MyTask
        {
            Id = myTaskId,
            Title = "Spike time Testcontainers"
        }));

        using var client = app.CreateClient();

        var expectedTask = await client.GetFromJsonAsync<MyTask>($"api/tasks/{myTaskId}");

        Assert.Equal("Spike time Testcontainers", expectedTask?.Title);
    }

    private async Task InitializeGetTasksDataAsync(WebApplicationFactory<Program> app)
    {
        using var scope = app.Services.CreateScope();

        using var db = scope.ServiceProvider.GetRequiredService<MyTasksDbContext>();
        db.MyTasks.Add(new MyTask
        {
            Id = myTaskId,
            Title = "Spike time Testcontainers"
        });

        await db.SaveChangesAsync();
    }
}
