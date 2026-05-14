/// <summary>
/// Mô tả một mục log được lưu.
/// </summary>
public class Log
{
    /// <summary>
    /// Khóa chính.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nội dung thông điệp log.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Thời điểm tạo (mặc định là thời điểm hiện tại).
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}