using Microsoft.EntityFrameworkCore;
using WebAPI.Model;

namespace WebAPI.Data
{
    // Fix: Inherit from Microsoft.EntityFrameworkCore.DbContext, not from a custom class.
    public class AppDbContext : DbContext
    {
        // Fix: Use primary constructor as suggested by IDE0290.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Fix: Mark DbSet properties as nullable to resolve CS8618.
        public DbSet<Order>? Orders { get; set; }
        public DbSet<User>? Users { get; set; }
    }
}
