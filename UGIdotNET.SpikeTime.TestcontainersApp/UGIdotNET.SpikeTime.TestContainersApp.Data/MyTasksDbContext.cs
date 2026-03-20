using Microsoft.EntityFrameworkCore;

namespace UGIdotNET.SpikeTime.TestContainersApp.Data;

public class MyTasksDbContext : DbContext
{
    public MyTasksDbContext(DbContextOptions<MyTasksDbContext> options)
        : base(options)
    {
    }

    public DbSet<MyTask> MyTasks { get; set; }
}
