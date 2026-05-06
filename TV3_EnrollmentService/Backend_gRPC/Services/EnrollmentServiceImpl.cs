using Grpc.Core;
using EnrollmentService.gRPC; 
using studentManagement.Data; 
using studentManagement.Models;

namespace studentManagement.Services
{
    public class EnrollmentServiceImpl : Enrollment.EnrollmentBase
    {
        private readonly EnrollmentDbContext _db;

        public EnrollmentServiceImpl(EnrollmentDbContext db)
        {
            _db = db;
        }

        public override async Task<EnrollResponse> RegisterCourse(EnrollRequest request, ServerCallContext context)
        {
            var record = new EnrollmentRecord
            {
                StudentId = request.StudentId, // kiểu string
                CourseId = request.CourseId,   // kiểu string
                EnrollmentDate = DateTime.Now,
                Status = "Success"
            };

            _db.Enrollments.Add(record);
            await _db.SaveChangesAsync();

            return new EnrollResponse
            {
                Success = true,
                Message = $"[TV3-Phú] Đã lưu đăng ký môn {request.CourseId} cho SV {request.StudentId} vào SQL Server!"
            };
        }

        public override async Task StreamEnrollmentStatus(StudentIdRequest request, IServerStreamWriter<StatusUpdate> responseStream, ServerCallContext context)
        {
            string[] steps = {
                "Kết nối Enrollment Service...",
                "Đang xác thực mã sinh viên: " + request.StudentId,
                "Kiểm tra điều kiện tiên quyết...",
                "Đang ghi dữ liệu vào SQL Server...",
                "Hoàn tất đăng ký!"
            };

            foreach (var step in steps)
            {
                if (context.CancellationToken.IsCancellationRequested) break;

                await responseStream.WriteAsync(new StatusUpdate
                {
                    Status = step,
                    Timestamp = DateTime.Now.ToString("HH:mm:ss")
                });

                await Task.Delay(1000); 
            }
        }
    }
}