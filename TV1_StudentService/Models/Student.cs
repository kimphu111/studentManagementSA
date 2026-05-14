// Lớp entity đại diện cho bảng Students trong DB
public class Student
{
    // Khóa chính (Id tự tăng)
    public int Id { get; set; }

    // Tên sinh viên
    public string Name { get; set; }

    // Email sinh viên
    public string Email { get; set; }

    // Tuổi
    public int Age { get; set; }
}