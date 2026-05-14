using Grpc.Core;
using NotificationService.Protos;

namespace NotificationService.Services;

/// <summary>
/// Triển khai dịch vụ gRPC nhận log và cung cấp endpoint streaming để client tải về
/// các log gần đây.
/// </summary>
public class MyNotificationService : Notification.NotificationBase
{
    /// <summary>
    /// Nhận một dòng log từ client và lưu vào <see cref="LogStore"/> trong bộ nhớ.
    /// Trả về <see cref="LogReply"/> đơn giản để báo thành công.
    /// </summary>
    /// <remarks>
    /// Phương thức chèn các mục log mới vào đầu danh sách để consumer có thể đọc
    /// theo thứ tự mới nhất trước. Thiết kế này đơn giản phục vụ demo/kiểm thử.
    /// </remarks>
    public override Task<LogReply> SendLog(LogRequest request, ServerCallContext context)
    {
        // Định dạng thời gian + nội dung để dễ đọc
        var time = DateTime.Now.ToString("HH:mm:ss");
        var logMessage = $"[{time}] {request.Message}";

        // Ghi ra console để dễ theo dõi phía server
        Console.WriteLine($"[CÓ BIẾN]: {request.Message}");

        // QUAN TRỌNG: thêm vào LogStore để các client streaming có thể lấy dữ liệu
        LogStore.Logs.Insert(0, logMessage);

        return Task.FromResult(new LogReply { Success = true });
    }

    /// <summary>
    /// Phát streaming tối đa <paramref name="request"/>.MaxItems dòng log về client.
    /// Mỗi dòng được gửi dưới dạng một thông điệp <see cref="LogMessage"/>.
    /// </summary>
    /// <remarks>
    /// Có một độ trễ nhỏ giữa các thông điệp để dễ quan sát hành vi streaming khi demo.
    /// </remarks>
    public override async Task DownloadLogStream(StreamRequest request, IServerStreamWriter<LogMessage> responseStream, ServerCallContext context)
    {
        // Lấy tối đa MaxItems từ kho log trong bộ nhớ
        var logsToSend = LogStore.Logs.Take(request.MaxItems).ToList();

        // Gửi từng dòng log dưới dạng thông điệp stream
        foreach (var log in logsToSend)
        {
            await responseStream.WriteAsync(new LogMessage { Content = log });
            await Task.Delay(1000); // Delay nhỏ cho dễ quan sát streaming
        }
    }
}