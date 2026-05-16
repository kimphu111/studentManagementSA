using Microsoft.EntityFrameworkCore;
using AuthGateway.Models;

namespace AuthGateway.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}