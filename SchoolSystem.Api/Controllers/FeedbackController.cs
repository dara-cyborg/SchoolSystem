using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.DTOs.Feedback;
using SchoolSystem.Core.Enums;
using SchoolSystem.Core.Extensions;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase {
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService) {
        _feedbackService = feedbackService;
    }

    /// <summary>
    /// Submit feedback for a report
    /// [parent]
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "ParentOnly")]
    public async Task<IActionResult> SubmitFeedback([FromBody] CreateFeedbackDto createDto, CancellationToken cancellationToken = default) {
        try {
            var parentUserId = User.GetUserId();
            var result = await _feedbackService.CreateAsync(createDto, parentUserId, cancellationToken);
            return CreatedAtAction(nameof(SubmitFeedback), result);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get all feedback for a specific report
    /// [super_admin, homeroom, parent]
    /// </summary>
    [HttpGet("report/{reportType}/{reportId}")]
    [Authorize]
    public async Task<IActionResult> GetFeedbackByReport(string reportType, int reportId, CancellationToken cancellationToken = default) {
        try {
            // Parse report type
            if (!Enum.TryParse<ReportType>(reportType, ignoreCase: true, out var parsedReportType)) {
                return BadRequest(new { message = "Invalid report type. Use 'monthly', 'semester', or 'yearly'." });
            }

            int? parentUserId = null;
            if (User.IsInRole("Parent")) {
                parentUserId = User.GetUserId();
            }

            var result = await _feedbackService.GetByReportAsync(parsedReportType, reportId, parentUserId, cancellationToken);
            return Ok(result);
        } catch (Exception ex) {
            return StatusCode(500, new { message = "An error occurred while retrieving feedback.", error = ex.Message });
        }
    }

    /// <summary>
    /// Get all feedback for a class
    /// [super_admin, homeroom]
    /// </summary>
    [HttpGet("class/{classId}")]
    [Authorize(Policy = "SuperAdminOrHomeroom")]
    public async Task<IActionResult> GetFeedbackByClass(int classId, CancellationToken cancellationToken = default) {
        try {
            var result = await _feedbackService.GetByClassAsync(classId, cancellationToken);
            return Ok(result);
        } catch (Exception ex) {
            return StatusCode(500, new { message = "An error occurred while retrieving feedback.", error = ex.Message });
        }
    }
}
