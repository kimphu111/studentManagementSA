using Microsoft.EntityFrameworkCore;
using studentManagement.Models;

namespace studentManagement.Data
{
    public class EnrollmentDbContext : DbContext
    {
        public EnrollmentDbContext(DbContextOptions<EnrollmentDbContext> options) : base(options) { }
        public DbSet<EnrollmentRecord> Enrollments { get; set; } = default!;
    }
}