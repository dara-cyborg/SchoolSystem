using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs.Attendance;
using SchoolSystem.Core.Enums;
using SchoolSystem.Core.Interfaces;
using SchoolSystem.Core.Models;

namespace SchoolSystem.Api.Services;

public class AttendanceService : IAttendanceService {
    private readonly AppDbContext _context;

    public AttendanceService(AppDbContext context) {
        _context = context;
    }

    public async Task<AttendanceDto> CreateAttendanceAsync(CreateAttendanceDto createDto, int teacherUserId, CancellationToken cancellationToken = default) {
        var studentExists = await _context.Students
            .AnyAsync(s => s.Id == createDto.StudentId, cancellationToken);

        if (!studentExists) {
            throw new InvalidOperationException($"Student with ID {createDto.StudentId} does not exist.");
        }

        var classSubject = await _context.ClassSubjects
            .FirstOrDefaultAsync(cs => cs.Id == createDto.ClassSubjectId, cancellationToken);

        if (classSubject == null) {
            throw new InvalidOperationException($"ClassSubject with ID {createDto.ClassSubjectId} does not exist.");
        }

        if (classSubject.TeacherUserId != teacherUserId) {
            throw new InvalidOperationException("You are not assigned to this class-subject.");
        }

        var attendance = new Attendance {
            StudentId = createDto.StudentId,
            ClassSubjectId = createDto.ClassSubjectId,
            Date = createDto.Date,
            Status = createDto.Status,
            CreatedAt = DateTime.UtcNow
        };

        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync(cancellationToken);

        return MapAttendanceToDto(attendance);
    }

    public async Task CreateBulkAttendanceAsync(BulkAttendanceDto bulkDto, int teacherUserId, CancellationToken cancellationToken = default) {
        if (bulkDto.Records == null || bulkDto.Records.Count == 0) {
            throw new InvalidOperationException("Bulk attendance records cannot be empty.");
        }

        var uniqueClassSubjectIds = bulkDto.Records
            .Select(r => r.ClassSubjectId)
            .Distinct()
            .ToList();

        var classSubjects = await _context.ClassSubjects
            .Where(cs => uniqueClassSubjectIds.Contains(cs.Id))
            .ToListAsync(cancellationToken);

        if (classSubjects.Count != uniqueClassSubjectIds.Count) {
            throw new InvalidOperationException("One or more ClassSubject IDs do not exist.");
        }

        var unauthorizedClassSubjects = classSubjects
            .Where(cs => cs.TeacherUserId != teacherUserId)
            .ToList();

        if (unauthorizedClassSubjects.Any()) {
            throw new InvalidOperationException("You are not assigned to one or more class-subjects.");
        }

        var studentIds = bulkDto.Records
            .Select(r => r.StudentId)
            .Distinct()
            .ToList();

        var existingStudents = await _context.Students
            .Where(s => studentIds.Contains(s.Id))
            .CountAsync(cancellationToken);

        if (existingStudents != studentIds.Count) {
            throw new InvalidOperationException("One or more Student IDs do not exist.");
        }

        var attendances = bulkDto.Records.Select(record => new Attendance {
            StudentId = record.StudentId,
            ClassSubjectId = record.ClassSubjectId,
            Date = record.Date,
            Status = record.Status,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        _context.Attendances.AddRange(attendances);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<AttendanceDto>?> GetStudentAttendanceAsync(int studentId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default) {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken);

        if (student == null) {
            return null;
        }

        var query = _context.Attendances
            .Where(a => a.StudentId == studentId)
            .AsQueryable();

        if (startDate.HasValue) {
            query = query.Where(a => a.Date >= startDate.Value);
        }

        if (endDate.HasValue) {
            query = query.Where(a => a.Date <= endDate.Value);
        }

        var attendances = await query
            .OrderBy(a => a.Date)
            .ToListAsync(cancellationToken);

        return attendances.Select(MapAttendanceToDto).ToList();
    }

    public async Task<AttendanceSummaryDto?> GetAttendanceSummaryAsync(int studentId, int month, short schoolYear, CancellationToken cancellationToken = default) {
        if (schoolYear < 1 || schoolYear > 9999) {
            throw new InvalidOperationException("Invalid school year.");
        }

        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken);

        if (student == null) {
            return null;
        }

        var startDate = new DateTime(schoolYear, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var attendanceRecords = await _context.Attendances
            .Where(a => a.StudentId == studentId &&
                        a.Date >= startDate &&
                        a.Date <= endDate)
            .ToListAsync(cancellationToken);

        var informedAbsences = attendanceRecords
            .Count(a => a.Status == AttendanceStatus.InformedAbsent);

        var uninformedAbsences = attendanceRecords
            .Count(a => a.Status == AttendanceStatus.UninformedAbsent);

        return new AttendanceSummaryDto {
            StudentId = student.Id,
            StudentName = student.Name,
            InformedAbsences = informedAbsences,
            UninformedAbsences = uninformedAbsences
        };
    }

    private static AttendanceDto MapAttendanceToDto(Attendance attendance) {
        return new AttendanceDto {
            Id = attendance.Id,
            StudentId = attendance.StudentId,
            ClassSubjectId = attendance.ClassSubjectId,
            Date = attendance.Date,
            Status = attendance.Status
        };
    }
}
