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

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


builder.WebHost.ConfigureKestrel(options =>
{
    // Cổng 5272 chỉ dành riêng cho Web / Razor Pages (HTTP/1.1)
    options.ListenAnyIP(5272, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    // Mở thêm một cổng mới (ví dụ 5273) CHỈ dành riêng cho gRPC (HTTP/2)
    options.ListenAnyIP(5273, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 4. Map gRPC
app.MapGrpcService<EnrollmentServiceImpl>();

// 5. Map Razor Pages
app.MapRazorPages();

app.MapGet("/", () => "Enrollment Service (TV3 - Phú) is running on port 5000... Mở /Enrollments để vào quản lý.");

app.Run();   