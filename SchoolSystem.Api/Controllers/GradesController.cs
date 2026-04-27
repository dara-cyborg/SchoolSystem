using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Core.DTOs.Grade;
using SchoolSystem.Core.Interfaces;

namespace SchoolSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "SuperAdminOnly")]
public class GradesController : ControllerBase {
    private readonly IAcademicService _academicService;

    public GradesController(IAcademicService academicService) {
        _academicService = academicService;
    }

    /// <summary>
    /// Get paginated list of grades
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetGrades([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default) {
        if (page < 1 || pageSize < 1) {
            return BadRequest(new { message = "Page and PageSize must be greater than 0" });
        }

        var result = await _academicService.GetGradesAsync(page, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific grade by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGrade(int id, CancellationToken cancellationToken = default) {
        var grade = await _academicService.GetGradeByIdAsync(id, cancellationToken);

        if (grade == null) {
            return NotFound(new { message = "Grade not found" });
        }

        return Ok(grade);
    }

    /// <summary>
    /// Create a new grade
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateGrade([FromBody] CreateGradeDto createGradeDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(createGradeDto.Name)) {
            return BadRequest(new { message = "Grade name is required" });
        }

        try {
            var grade = await _academicService.CreateGradeAsync(createGradeDto, cancellationToken);
            return CreatedAtAction(nameof(GetGrade), new { id = grade.Id }, grade);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing grade
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGrade(int id, [FromBody] UpdateGradeDto updateGradeDto, CancellationToken cancellationToken = default) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(updateGradeDto.Name)) {
            return BadRequest(new { message = "Grade name is required" });
        }

        try {
            var success = await _academicService.UpdateGradeAsync(id, updateGradeDto, cancellationToken);

            if (!success) {
                return NotFound(new { message = "Grade not found" });
            }

            var grade = await _academicService.GetGradeByIdAsync(id, cancellationToken);
            return Ok(grade);
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a grade
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGrade(int id, CancellationToken cancellationToken = default) {
        try {
            var success = await _academicService.DeleteGradeAsync(id, cancellationToken);

            if (!success) {
                return NotFound(new { message = "Grade not found" });
            }

            return NoContent();
        } catch (InvalidOperationException ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
