using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Api.Data;
using SchoolSystem.Core.DTOs.Report;
using SchoolSystem.Core.Extensions;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/monthly-reports")]
public class MonthlyReportsController : ControllerBase
{
    private readonly IMonthlyReportService _monthlyReportService;
    private readonly AppDbContext _context;

    public MonthlyReportsController(IMonthlyReportService monthlyReportService, AppDbContext context)
    {
        _monthlyReportService = monthlyReportService;
        _context = context;
    }

    /// <summary>
    /// Submit a monthly report for a class
    /// [homeroom]
    /// </summary>
    [HttpPost("submit")]
    [Authorize(Policy = "HomeroomOnly")]
    public async Task<IActionResult> SubmitMonthlyReport([FromBody] SubmitMonthlyReportDto submitDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var homeroomUserId = User.GetUserId();
            var result = await _monthlyReportService.SubmitReportAsync(submitDto, homeroomUserId, cancellationToken);
            return CreatedAtAction(nameof(GetMonthlyReport), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get a monthly report by class, month, and school year
    /// [homeroom]
    /// </summary>
    [HttpGet("by-class")]
    [Authorize(Policy = "HomeroomOnly")]
    public async Task<IActionResult> GetMonthlyReportByClass(
    [FromQuery] int classId,
    [FromQuery] short month,
    [FromQuery] short schoolYear,
    CancellationToken cancellationToken = default)
    {
        var result = await _monthlyReportService.GetReportByClassAsync(classId, month, schoolYear, cancellationToken);

        if (result == null)
            return NotFound(new { message = "No report found for the specified class, month, and school year." });

        return Ok(result);
    }

    /// <summary>
    /// Get a monthly report by ID
    /// [super_admin, homeroom, parent]
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetMonthlyReport(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _monthlyReportService.GetReportAsync(id, cancellationToken);

            if (result == null)
            {
                return NotFound(new { message = $"Monthly report with ID {id} does not exist." });
            }

            if (User.IsInRole("Parent"))
            {
                var parentUserId = User.GetUserId();
                var report = await _context.MonthlyReports
                    .Include(mr => mr.Class)
                    .Include(mr => mr.Entries)
                    .FirstOrDefaultAsync(mr => mr.Id == id, cancellationToken);

                if (report == null)
                {
                    return NotFound(new { message = $"Monthly report with ID {id} does not exist." });
                }

                var hasAccessToClass = await _context.ParentStudents
                    .Include(ps => ps.Student)
                    .AnyAsync(ps =>
                        ps.ParentUserId == parentUserId &&
                        ps.Student.ClassId == report.ClassId,
                        cancellationToken);

                if (!hasAccessToClass)
                {
                    return Forbid();
                }
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the report.", error = ex.Message });
        }
    }
}