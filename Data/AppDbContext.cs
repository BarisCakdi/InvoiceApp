using InvoiceApp.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}