using Microsoft.EntityFrameworkCore;
using Core.Constants;
using Core.Entities;

namespace Data.Contexts;

public class AppDbContext : DbContext 
{

    public DbSet<Group> Groups { get; set; }
    public DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionStrings.MSSQL_CONNECTION);
    }
}
