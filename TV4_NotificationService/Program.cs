using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using NotificationService.Services;

/*
 Khởi tạo host đơn giản cho ứng dụng gRPC NotificationService.
 - Cấu hình Kestrel lắng nghe cổng 5167 với HTTP/2 (yêu cầu bởi gRPC).
 - Đăng ký EF Core `AppDbContext` với kết nối MySQL lấy từ cấu hình.
 - Thêm dịch vụ gRPC và ánh xạ lớp `MyNotificationService` để xử lý các cuộc gọi.
*/
var builder = WebApplication.CreateBuilder(args);

// Cấu hình Kestrel để hỗ trợ HTTP/2 cho client gRPC
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5167, o =>
    {
        o.Protocols = HttpProtocols.Http2;
    });
});

// Đăng ký DbContext của ứng dụng, sử dụng chuỗi kết nối "Default"
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("Default"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default"))));

// Thêm dịch vụ gRPC cho server
builder.Services.AddGrpc();

var app = builder.Build();

// Ánh xạ lớp dịch vụ gRPC để các cuộc gọi tới được xử lý đúng
app.MapGrpcService<MyNotificationService>();

// Endpoint HTTP đơn giản để thông báo cho client không phải gRPC
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();