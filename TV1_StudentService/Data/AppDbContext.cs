using Microsoft.EntityFrameworkCore;

// DbContext của ứng dụng, cấu hình các DbSet tương ứng với các bảng
public class AppDbContext : DbContext
{
    // Constructor: nhận options (connection string, provider, v.v.) từ DI
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    // DbSet tương ứng với bảng Students trong DB
    public DbSet<Student> Students { get; set; }
}