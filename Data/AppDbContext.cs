using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Data;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}