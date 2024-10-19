using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Data;

public class AppDbContext : DbContext
{
    // public DbSet<Teacher> Teachers { get; set; }
    // public DbSet<User> Users { get; set; }
    // public DbSet<ProgramModel> Programs { get; set; }
    // public DbSet<ManyToManyUserProgram> ManyToManyUserPrograms { get; set; }
    // public DbSet<History> Histories { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}