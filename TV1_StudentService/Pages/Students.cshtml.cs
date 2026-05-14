using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentService.Protos; // THÊM DÒNG NÀY ĐỂ LẤY CẤU TRÚC TIN NHẮN gRPC

namespace StudentService.Pages;

// PageModel cho trang /Students
public class StudentsModel : PageModel
{
    // DbContext để tương tác DB (Students table)
    private readonly AppDbContext _db;

    // gRPC client để gửi log/notification tới Notification service
    private readonly Notification.NotificationClient _grpcClient;

    // Constructor: DI sẽ cung cấp `AppDbContext` và `Notification.NotificationClient`
    public StudentsModel(AppDbContext db, Notification.NotificationClient grpcClient)
    {
        _db = db;
        _grpcClient = grpcClient;
    }

    // Danh sách sinh viên hiển thị trên trang
    public List<Student> StudentList { get; set; } = new();

    // Property được bind từ form (thêm / sửa)
    [BindProperty]
    public Student InputStudent { get; set; } = new();

    // OnGet: hiển thị trang, hỗ trợ editId để load dữ liệu khi sửa
    public void OnGet(int? editId)
    {
        if (editId.HasValue)
        {
            // Tìm sinh viên theo Id để hiển thị trong form sửa
            var sv = _db.Students.Find(editId.Value);
            if (sv != null) InputStudent = sv;
        }

        // Lấy 50 bản ghi gần nhất để hiển thị
        StudentList = _db.Students.OrderByDescending(s => s.Id).Take(50).ToList();
    }

    // OnPostSave: xử lý form Thêm / Cập nhật
    // - Nếu InputStudent.Id == 0 -> thêm mới, ngược lại update
    // - Sau khi lưu DB, gửi 1 message log qua gRPC (không bắt buộc phải thành công)
    public async Task<IActionResult> OnPostSave()
    {
        string logMsg = ""; // Nội dung log sẽ gửi qua gRPC

        if (InputStudent.Id == 0)
        {
            _db.Students.Add(InputStudent);
            logMsg = $"Hệ thống vừa THÊM MỚI sinh viên: {InputStudent.Name}";
        }
        else
        {
            _db.Students.Update(InputStudent);
            logMsg = $"Hệ thống vừa CẬP NHẬT thông tin sinh viên ID {InputStudent.Id}";
        }
        
        _db.SaveChanges(); // Lưu vào DB của Student trước

        // Gọi gRPC để gửi log; nếu lỗi thì bỏ qua để không làm sập web
        try 
        {
            await _grpcClient.SendLogAsync(new LogRequest { Message = logMsg });
        } 
        catch 
        { 
            // Nếu NotificationService không sẵn sàng, ta bỏ qua lỗi
        }

        return RedirectToPage("/Students"); 
    }

    // OnPostDelete: xử lý xóa theo id và gửi log
    public async Task<IActionResult> OnPostDelete(int id)
    {
        var sv = _db.Students.Find(id);
        if (sv != null)
        {
            string logMsg = $"Hệ thống vừa XÓA sinh viên: {sv.Name}";
            
            _db.Students.Remove(sv);
            _db.SaveChanges();

            // Gửi log qua gRPC (không bắt buộc)
            try 
            {
                await _grpcClient.SendLogAsync(new LogRequest { Message = logMsg });
            } 
            catch { }
        }
        return RedirectToPage("/Students");
    }
}