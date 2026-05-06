namespace studentManagement.Models
{
    public class EnrollmentRecord
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        public string? Status { get; set; }
    }
}