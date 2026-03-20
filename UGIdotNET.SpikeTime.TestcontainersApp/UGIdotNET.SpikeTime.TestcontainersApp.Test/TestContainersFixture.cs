using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace UGIdotNET.SpikeTime.TestcontainersApp.Test;

public class TestContainersFixture : IAsyncLifetime
{
    private MsSqlContainer _sqlServerContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest").Build();

    private RedisContainer _redisContainer = new RedisBuilder("docker.io/library/redis:8.2").Build();

    public string SqlServerConnectionString
    {
        get
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(_sqlServerContainer.GetConnectionString())
            {
                InitialCatalog = "MyTasksTestDb"
            };

            return connectionStringBuilder.ConnectionString; 
        }
    }

    public string RedisConnectionString => _redisContainer.GetConnectionString();

    public async Task DisposeAsync()
    {
        await _sqlServerContainer.DisposeAsync();
        await _redisContainer.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        await _sqlServerContainer.StartAsync();
        await _redisContainer.StartAsync();
    }
}
