using Microsoft.AspNetCore.Mvc;

// Controller API đơn giản để quản lý `Student` qua HTTP
[ApiController]
[Route("api/students")]
public class StudentController : ControllerBase
{
    // DbContext được inject để tương tác với DB
    private readonly AppDbContext _db;

    // Constructor: nhận `AppDbContext` từ DI container
    public StudentController(AppDbContext db)
    {
        _db = db;
    }

    // LẤY DAN SÁCH (Hiện người mới nhất lên đầu)
    // - HTTP GET api/students
    // - Trả về danh sách `Student`, sắp theo Id giảm dần (mới nhất trước)
    [HttpGet]
    public IActionResult GetAll()
    {
        // OrderByDescending: sắp theo Id giảm dần, ToList() để materialize kết quả
        return Ok(_db.Students.OrderByDescending(s => s.Id).ToList());
    }

    // THÊM MỚI
    // - HTTP POST api/students
    // - Body chứa `Student` mới, được thêm vào DB và trả về đối tượng đã tạo
    [HttpPost]
    public IActionResult Create(Student s)
    {
        _db.Students.Add(s);
        // SaveChanges viết thay đổi vào DB
        _db.SaveChanges();
        return Ok(s);
    }

    // SỬA THÔNG TIN
    // - HTTP PUT api/students/{id}
    // - Tìm student theo id, cập nhật trường cần thiết, rồi lưu
    [HttpPut("{id}")]
    public IActionResult Update(int id, Student s)
    {
        // Find: tìm theo khóa chính
        var sv = _db.Students.Find(id);
        if (sv == null) return NotFound();

        // Cập nhật các trường (ở đây chỉ Name và Email)
        sv.Name = s.Name;
        sv.Email = s.Email;
        _db.SaveChanges();
        // Trả về đối tượng đã được cập nhật
        return Ok(sv);
    }

    // XÓA
    // - HTTP DELETE api/students/{id}
    // - Xóa bản ghi nếu tồn tại
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var sv = _db.Students.Find(id);
        if (sv == null) return NotFound();

        _db.Students.Remove(sv);
        _db.SaveChanges();
        // Trả về HTTP 200 OK (không có nội dung)
        return Ok();
    }
}