using Microsoft.EntityFrameworkCore;
using SparekopiAdmin.Models;

namespace SparekopiAdmin.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ServiceItem> ServiceItems { get; set; }
}