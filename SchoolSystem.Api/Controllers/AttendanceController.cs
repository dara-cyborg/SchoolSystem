using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.DTOs.Attendance;
using SchoolSystem.Core.Extensions;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase {
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService) {
        _attendanceService = attendanceService;
    }

    /// <summary>
    /// Create a single attendance record
    /// [teacher]
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "TeacherOnly")]
    public async Task<IActionResult> CreateAttendance([FromBody] CreateAttendanceDto createDto, CancellationToken cancellationToken = default) {
        try {
            var teacherUserId = User.GetUserId();
            var result = await _attendanceService.CreateAttendanceAsync(createDto, teacherUserId, cancellationToken);
            return CreatedAtAction(nameof(GetStudentAttendance), new { studentId = result.StudentId }, result);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create multiple attendance records in bulk
    /// [teacher]
    /// </summary>
    [HttpPost("bulk")]
    [Authorize(Policy = "TeacherOnly")]
    public async Task<IActionResult> CreateBulkAttendance([FromBody] BulkAttendanceDto bulkDto, CancellationToken cancellationToken = default) {
        try {
            var teacherUserId = User.GetUserId();
            await _attendanceService.CreateBulkAttendanceAsync(bulkDto, teacherUserId, cancellationToken);
            return NoContent();
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get attendance records for a specific student with optional date filtering
    /// [super_admin, homeroom, teacher]
    /// </summary>
    [HttpGet("student/{studentId}")]
    [Authorize(Policy = "SuperAdminOrHomeroomOrTeacher")]
    public async Task<IActionResult> GetStudentAttendance(int studentId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null, CancellationToken cancellationToken = default) {
        var result = await _attendanceService.GetStudentAttendanceAsync(studentId, startDate, endDate, cancellationToken);
        if (result == null) {
            return NotFound(new { message = $"Student with ID {studentId} does not exist." });
        }
        return Ok(result);
    }

    /// <summary>
    /// Get attendance summary for a student in a specific month and school year
    /// [all authenticated roles]
    /// </summary>
    [HttpGet("summary/{studentId}")]
    [Authorize]
    public async Task<IActionResult> GetAttendanceSummary(int studentId, [FromQuery] int month, [FromQuery] short schoolYear, CancellationToken cancellationToken = default) {
        if (month < 1 || month > 12) {
            return BadRequest(new { message = "Month must be between 1 and 12" });
        }

        var result = await _attendanceService.GetAttendanceSummaryAsync(studentId, month, schoolYear, cancellationToken);
        if (result == null) {
            return NotFound(new { message = $"Student with ID {studentId} does not exist." });
        }
        return Ok(result);
    }
}
