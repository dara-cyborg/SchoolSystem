using SchoolSystem.Core.Enums;

namespace SchoolSystem.Core.DTOs.Attendance;

public class AttendanceDto {
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassSubjectId { get; set; }
    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }
}

public class CreateAttendanceDto {
    public int StudentId { get; set; }
    public int ClassSubjectId { get; set; }
    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }
}

public class BulkAttendanceDto {
    public List<CreateAttendanceDto> Records { get; set; } = new();
}

public class AttendanceSummaryDto {
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int InformedAbsences { get; set; }
    public int UninformedAbsences { get; set; }
}