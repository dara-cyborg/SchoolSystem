using SchoolSystem.Core.DTOs.Attendance;

namespace SchoolSystem.Core.Interfaces;

public interface IAttendanceService {
    Task<AttendanceDto> CreateAttendanceAsync(CreateAttendanceDto createDto, int teacherUserId, CancellationToken cancellationToken = default);
    Task CreateBulkAttendanceAsync(BulkAttendanceDto bulkDto, int teacherUserId, CancellationToken cancellationToken = default);
    Task<List<AttendanceDto>?> GetStudentAttendanceAsync(int studentId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task<AttendanceSummaryDto?> GetAttendanceSummaryAsync(int studentId, int month, short schoolYear, CancellationToken cancellationToken = default);
}