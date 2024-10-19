using InvoiceApp.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    
    public DbSet<Item> Items { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}