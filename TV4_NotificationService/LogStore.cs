namespace NotificationService;

/// <summary>
/// Kho lưu trữ log trong bộ nhớ (RAM).
/// Đây là một danh sách đơn giản dùng để các yêu cầu streaming có thể đọc các log gần đây.
/// Thiết kế tối giản cho mục đích demo/kiểm thử; trong môi trường production nên dùng
/// lưu trữ bền vững hoặc bộ đệm có giới hạn để tránh tăng bộ nhớ không kiểm soát.
/// </summary>
public static class LogStore
{
    /// <summary>
    /// Danh sách các dòng log, mới nhất ở đầu (chèn vào index 0).
    /// Các consumer sẽ đọc từ collection này khi trả về streaming.
    /// </summary>
    public static List<string> Logs { get; set; } = new List<string>();
}