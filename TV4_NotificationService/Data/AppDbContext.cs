using Microsoft.EntityFrameworkCore;

/// <summary>
/// DbContext của EF Core cho dịch vụ notification.
/// Chứa DbSet <see cref="Log"/> để lưu các mục log nếu sử dụng cơ sở dữ liệu quan hệ.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    /// <summary>
    /// Bảng Logs (nếu sử dụng DB quan hệ). Mô hình các mục log được lưu.
    /// </summary>
    public DbSet<Log> Logs { get; set; }
}