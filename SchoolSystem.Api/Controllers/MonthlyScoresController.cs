using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.DTOs.Score;
using SchoolSystem.Core.Extensions;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MonthlyScoresController : ControllerBase {
    private readonly IMonthlyScoreService _monthlyScoreService;

    public MonthlyScoresController(IMonthlyScoreService monthlyScoreService) {
        _monthlyScoreService = monthlyScoreService;
    }

    /// <summary>
    /// Submit a monthly score for a student
    /// [teacher]
    /// </summary>
    [HttpPost("submit")]
    [Authorize(Policy = "TeacherOnly")]
    public async Task<IActionResult> SubmitMonthlyScore([FromBody] SubmitMonthlyScoreDto submitDto, CancellationToken cancellationToken = default) {
        try {
            var teacherUserId = User.GetUserId();
            var result = await _monthlyScoreService.SubmitAsync(submitDto, teacherUserId, cancellationToken);
            return CreatedAtAction(nameof(SubmitMonthlyScore), result);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update a monthly score (homeroom only)
    /// [homeroom]
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "HomeroomOnly")]
    public async Task<IActionResult> UpdateMonthlyScore(int id, [FromBody] decimal finalScore, CancellationToken cancellationToken = default) {
        try {
            var result = await _monthlyScoreService.UpdateAsync(id, finalScore, cancellationToken);
            if (result == null) {
                return NotFound(new { message = $"Monthly score with ID {id} does not exist." });
            }
            return Ok(result);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
