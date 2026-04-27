using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.Models;

public class Attendance : BaseEntity{
    public int StudentId { get; set; }
    public int ClassSubjectId { get; set; }
    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }

    // Navigation properties
    public Student Student { get; set; } = null!;
    public ClassSubject ClassSubject { get; set; } = null!;
}