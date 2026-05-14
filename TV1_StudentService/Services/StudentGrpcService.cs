using Grpc.Core;
using StudentService;

// Implementation của gRPC service được tạo từ proto (StudentGrpc)
// Trả về thông tin `Student` theo yêu cầu gRPC
public class StudentGrpcService : StudentGrpc.StudentGrpcBase
{
    // DbContext để truy vấn dữ liệu
    private readonly AppDbContext _db;

    // Constructor: nhận `AppDbContext` từ DI
    public StudentGrpcService(AppDbContext db)
    {
        _db = db;
    }

    // Ghi đè method GetStudent được định nghĩa trong proto
    // - `request.Id` là id sinh viên cần lấy
    // - Trả về `StudentResponse` (rỗng nếu không tìm thấy)
    public override Task<StudentResponse> GetStudent(StudentRequest request, ServerCallContext context)
    {
        var s = _db.Students.Find(request.Id);

        // Nếu không tìm thấy, trả về response rỗng (các trường giữ giá trị mặc định)
        if (s == null)
            return Task.FromResult(new StudentResponse());

        // Mapping từ entity `Student` sang `StudentResponse` (bản tin gRPC)
        return Task.FromResult(new StudentResponse
        {
            Id = s.Id,
            Name = s.Name,
            Email = s.Email,
            Age = s.Age
        });
    }
}