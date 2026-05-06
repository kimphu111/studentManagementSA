using studentManagement.Data;
using studentManagement.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình Database
builder.Services.AddDbContext<EnrollmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Thêm gRPC
builder.Services.AddGrpc();

// 3. Thêm Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 4. Map gRPC
app.MapGrpcService<EnrollmentServiceImpl>();

// 5. Map Razor Pages
app.MapRazorPages();

app.MapGet("/", () => "Enrollment Service (TV3 - Phú) is running on port 5000... Mở /Enrollments để vào quản lý.");

app.Run();   