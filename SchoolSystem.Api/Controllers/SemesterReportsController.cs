using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs.Report;
using SchoolSystem.Core.Extensions;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/semester-reports")]
public class SemesterReportsController : ControllerBase {
    private readonly ISemesterReportService _semesterReportService;
    private readonly AppDbContext _context;

    public SemesterReportsController(ISemesterReportService semesterReportService, AppDbContext context) {
        _semesterReportService = semesterReportService;
        _context = context;
    }

    /// <summary>
    /// Submit a semester report for a class
    /// [homeroom]
    /// </summary>
    [HttpPost("submit")]
    [Authorize(Policy = "HomeroomOnly")]
    public async Task<IActionResult> SubmitSemesterReport([FromBody] SubmitSemesterReportDto submitDto, CancellationToken cancellationToken = default) {
        try {
            var homeroomUserId = User.GetUserId();
            var result = await _semesterReportService.SubmitReportAsync(submitDto, homeroomUserId, cancellationToken);
            return CreatedAtAction(nameof(GetSemesterReport), new { id = result.Id }, result);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get a semester report by ID
    /// [super_admin, homeroom, parent]
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetSemesterReport(int id, CancellationToken cancellationToken = default) {
        try {
            var result = await _semesterReportService.GetReportAsync(id, cancellationToken);
            if (result == null) {
                return NotFound(new { message = $"Semester report with ID {id} does not exist." });
            }

            // Parent can only view reports for their linked children's class
            if (User.IsInRole("Parent")) {
                var parentUserId = User.GetUserId();
                var report = await _context.SemesterReports
                    .Include(sr => sr.Class)
                    .Include(sr => sr.Entries)
                    .FirstOrDefaultAsync(sr => sr.Id == id, cancellationToken);

                if (report == null) {
                    return NotFound(new { message = $"Semester report with ID {id} does not exist." });
                }

                var hasAccessToClass = await _context.ParentStudents
                    .Include(ps => ps.Student)
                    .AnyAsync(ps => 
                        ps.ParentUserId == parentUserId &&
                        ps.Student.ClassId == report.ClassId,
                        cancellationToken);

                if (!hasAccessToClass) {
                    return Forbid();
                }
            }

            return Ok(result);
        } catch (Exception ex) {
            return StatusCode(500, new { message = "An error occurred while retrieving the report.", error = ex.Message });
        }
    }
}
