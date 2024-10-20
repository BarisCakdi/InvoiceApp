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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .HasOne(i => i.Invoice)
            .WithMany(a => a.Items)
            .HasForeignKey(i => i.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Invoice>() 
            .HasOne(a => a.User)
            .WithMany(s => s.Invoices)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        
    }
}